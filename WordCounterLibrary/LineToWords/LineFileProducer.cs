using Microsoft.Extensions.Logging;
using shortid;

namespace WordCounterLibrary.LineToWords
{
  internal class LineFileProducer : ILineFileProducer
  {
    private ILogger<LineFileProducer> _logger;
    private readonly string _producerId;
    private readonly IFileReader _fileReader;

    public LineFileProducer(ILogger<LineFileProducer> logger, IBufferWriter wordStorage, IFileReader fileReader) //TODO fix
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _producerId = ShortId.Generate();
      _fileReader = fileReader ?? throw new ArgumentNullException(nameof(fileReader));
    }

    public async Task ProduceAsync(IEnumerable<string> filePaths, CancellationToken cancellationToken = default)
    {
      foreach (var filePath in filePaths)
      {
        _logger.LogInformation("Producer '{producerId}' Started to publish from file '{filePath}'", _producerId, filePath);
        await _fileReader.WriteFileContentToBufferAsync(filePath);
        _logger.LogInformation("Producer '{producerId}' finished to publish", _producerId);
      }
    }
  }
}
