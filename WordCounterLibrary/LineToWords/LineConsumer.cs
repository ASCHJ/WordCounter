using Microsoft.Extensions.Logging;
using shortid;

namespace WordCounterLibrary.LineToWords
{
  internal class LineConsumer
  {
    private readonly ILogger<LineConsumer> _logger;
    private readonly string _consumerId;
    private readonly IBufferReader _reader;
    private readonly IWordStorage _wordStorage;

    public LineConsumer(ILogger<LineConsumer> logger, IBufferReader reader, IWordStorage wordStorage)
    {
      _logger = logger;
      _consumerId = ShortId.Generate();
      _reader = reader ?? throw new ArgumentNullException(nameof(reader));
      _wordStorage = wordStorage ?? throw new ArgumentNullException(nameof(wordStorage));
    }

    public async Task ConsumeAsync(CancellationToken cancellationToken = default)
    {
      _logger.LogInformation("Consumer '{ConsumerId}' Started to consuming", _consumerId);

      try
      {
        await foreach (var line in _reader.ReadAllAsync(cancellationToken).ConfigureAwait(false))
        {
          _logger.LogDebug("Consumer '{ConsumerId}' consuming '{line}'", _consumerId, line);
          _wordStorage.AddOrUpdate(line, 1);
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
