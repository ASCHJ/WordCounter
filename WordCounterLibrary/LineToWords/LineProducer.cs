using Microsoft.Extensions.Logging;
using shortid;

namespace WordCounterLibrary.LineToWords
{
  internal class LineProducer
  {
    private readonly ILogger<LineProducer> _logger;
    private readonly string _producerId;
    private readonly IBufferWriter _writer;

    public LineProducer(ILogger<LineProducer> logger, IBufferWriter writer)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _producerId = ShortId.Generate();
      _writer = writer ?? throw new ArgumentNullException(nameof(writer));
    }

    public async Task PublishAsync(string message, CancellationToken cancellationToken = default)
    {
      _logger.LogDebug("Producer '{producerId}' Started to publish", _producerId);
      await _writer.WriteAsync(message, cancellationToken).ConfigureAwait(false);
      _logger.LogDebug("Producer '{producerId}' finished to publish", _producerId);
    }
  }
}
