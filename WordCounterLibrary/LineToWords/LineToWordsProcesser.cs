using Microsoft.Extensions.Logging;
using WordCounterLibrary.Services;

namespace WordCounterLibrary.LineToWords
{
  public class LineToWordsProcessor : IWordsProcessor
  {
    private static readonly int BufferCapacity = 500;

    private readonly ILogger<LineToWordsProcessor> _lineToWordsProcessorLogger;
    private readonly IBufferStorage _internalStorage;
    private readonly ILineFileProducerService _lineFileProducerCreator;
    private readonly ILineConsumerService _consumerCreator;

    internal LineToWordsProcessor(ILogger<LineToWordsProcessor> logger, IBufferStorage internalStorage, ILineFileProducerService lineFileProducerCreator, ILineConsumerService consumerCreator)
    {
      _lineToWordsProcessorLogger = logger;
      _internalStorage = internalStorage ?? throw new ArgumentNullException(nameof(internalStorage));
      _lineFileProducerCreator = lineFileProducerCreator ?? throw new ArgumentNullException(nameof(lineFileProducerCreator));
      _consumerCreator = consumerCreator ?? throw new ArgumentNullException(nameof(consumerCreator));
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
        var lineFileProducer = _lineFileProducerCreator.Creator()
            .ProduceAsync(filePaths, cancellationToken);

        return lineFileProducer;
      }).ToArray();

      return producerTasks;
    }

    private Task[] ConsumerTasks(int consumersCount, CancellationToken cancellationToken)
    {
      var consumerTasks = Enumerable.Range(1, consumersCount)
          .Select(i => _consumerCreator.Creator()
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
