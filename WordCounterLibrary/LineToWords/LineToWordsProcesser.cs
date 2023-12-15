using Microsoft.Extensions.Logging;
using WordCounterLibrary.Format;

namespace WordCounterLibrary.LineToWords
{
  public class LineToWordsProcessor
  {
    private static readonly int BufferCapacity = 500;

    private readonly ILoggerFactory _loggerFactory;

    private readonly ILogger<LineToWordsProcessor> _lineToWordsProcessorLogger;
    private readonly ILogger<LineConsumer> _lineConsumerLogger;
    private readonly ILogger<LineFileProducer> _lineFileProducerLogger;
    private readonly IFileReader _fileReader;
    private readonly IBufferStorage _internalStorage;
    private readonly IWordStorage _wordStorage;

    public LineToWordsProcessor(ILoggerFactory loggerFactory, IWordStorage wordStorage)
        : this(loggerFactory, new StreamFileReader(loggerFactory.CreateLogger<StreamFileReader>(), wordStorage, new LipsumLineFormatParser()), wordStorage, new ChannelAsBuffer(BufferCapacity))
    {
    }

    internal LineToWordsProcessor(ILoggerFactory loggerFactory, IFileReader fileReader, IWordStorage wordStorage, IBufferStorage internalStorage)
    {
      _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory)); //TODO: Do I want a IoC dependency. 

      _lineToWordsProcessorLogger = _loggerFactory.CreateLogger<LineToWordsProcessor>();
      _lineConsumerLogger = _loggerFactory.CreateLogger<LineConsumer>();
      _lineFileProducerLogger = _loggerFactory.CreateLogger<LineFileProducer>();
      _fileReader = fileReader ?? throw new ArgumentNullException(nameof(fileReader));
      _internalStorage = internalStorage ?? throw new ArgumentNullException(nameof(internalStorage));
      _wordStorage = wordStorage ?? throw new ArgumentNullException(nameof(wordStorage));
    }

    public async Task Execute(int producersCount, int consumersCount, IEnumerable<string> filePaths, CancellationToken cancellationToken)
    {
      _lineToWordsProcessorLogger.LogInformation(
          "Starting to execute {producersCount} producers, {consumersCount} consumers, files {fileCount}, buffer capacity {bufferCapacity}",
          producersCount, consumersCount, filePaths.Count(), BufferCapacity);

      var tasks = ConsumerTasks(consumersCount, cancellationToken)
          .Append(ProduceAsync(filePaths, producersCount, cancellationToken))
          .ToArray();

      await Task.WhenAll(tasks);

      _lineToWordsProcessorLogger.LogInformation("Done producing and consuming");
    }

    private async Task ProduceAsync(IEnumerable<string> filePaths, int producersCount, CancellationToken cancellationToken)
    {
      Task[] producerTasks = ProducerTasks(filePaths, producersCount, cancellationToken);

      await Task.WhenAll(producerTasks);

      _lineToWordsProcessorLogger.LogInformation("Producers complete");
      _internalStorage.Writer.Complete();

      _lineToWordsProcessorLogger.LogDebug("Waiting for consumers to complete.");
      await _internalStorage.Reader.Completion;

      _lineToWordsProcessorLogger.LogInformation("Completion");
    }

    private Task[] ProducerTasks(IEnumerable<string> filePaths, int producersCount, CancellationToken cancellationToken)
    {
      //TODO: check if producers > filepaths
      var distributedPaths = DistributeElements(producersCount, filePaths);
      var producerTasks = distributedPaths.Select(filePaths =>
      {
        var lineFileProducer = new LineFileProducer(_lineFileProducerLogger, _wordStorage, _fileReader, filePaths) //TODO: Fix
            .ProduceAsync(cancellationToken);

        return lineFileProducer;
      }).ToArray();

      return producerTasks;
    }

    private Task[] ConsumerTasks(int consumersCount, CancellationToken cancellationToken)
    {
      var consumerTasks = Enumerable.Range(1, consumersCount)
          .Select(i => new LineConsumer(_lineConsumerLogger, _internalStorage.Reader, _wordStorage) //TODO: Fix
              .ConsumeAsync(cancellationToken))
          .ToArray();

      return consumerTasks;
    }

    private static IEnumerable<string>[] DistributeElements(int producersCount, IEnumerable<string> filePaths) //TODO: move to helper class?
    {
      List<string>[] distributedElements = new List<string>[producersCount];
      for (int i = 0; i < producersCount; i++)
      {
        distributedElements[i] = new List<string>();
      }

      int groupIndex = 0;
      foreach (var element in filePaths)
      {
        distributedElements[groupIndex].Add(element);
        groupIndex = (groupIndex + 1) % producersCount;
      }

      return distributedElements;
    }
  }
}
