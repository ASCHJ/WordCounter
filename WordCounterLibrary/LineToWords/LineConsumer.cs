using Microsoft.Extensions.Logging;
using shortid;
using WordCounterLibrary.Format;
using WordCounterLibrary.Repository;

namespace WordCounterLibrary.LineToWords
{
  internal class LineConsumer : ILineConsumer
  {
    private readonly ILogger<LineConsumer> _logger;
    private readonly string _consumerId;
    private readonly IBufferReader _reader;
    private readonly ILineFormatParser _lineFormatParser;
    private readonly IWordRepository _wordStorage;

    public LineConsumer(ILogger<LineConsumer> logger, IBufferReader reader, ILineFormatParser lineFormatParser, IWordRepository wordStorage)
    {
      _logger = logger;
      _consumerId = ShortId.Generate();
      _reader = reader ?? throw new ArgumentNullException(nameof(reader));
      _lineFormatParser = lineFormatParser ?? throw new ArgumentNullException(nameof(lineFormatParser));
      _wordStorage = wordStorage ?? throw new ArgumentNullException(nameof(wordStorage));
    }

    public async Task ConsumeAsync(CancellationToken cancellationToken = default)
    {
      _logger.LogInformation("Consumer '{ConsumerId}' Started to consuming", _consumerId);

      try
      {
        await foreach (var line in _reader.ReadAllAsync(cancellationToken))
        {
          var wordKeyPairs = _lineFormatParser.GetWordKeyPairs(line);
          foreach (var wordKeyPair in wordKeyPairs)
          {
            _wordStorage.AddOrUpdate(wordKeyPair.Key, wordKeyPair.Value);
          }
        }
      }
      catch (OperationCanceledException)
      {
        _logger.LogInformation("Consumer {ConsumerId} Canceled", _consumerId);
      }

      _logger.LogInformation("Consumer '{ConsumerId}' Stopped to consuming", _consumerId);
    }
  }
}
