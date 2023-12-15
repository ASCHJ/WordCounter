using Microsoft.Extensions.Logging;
using shortid;

namespace WordCounterLibrary.LineToWords
{
  internal class LineFileProducer
  {
    private ILogger<LineFileProducer> _logger;
    private readonly string _producerId;
    private readonly IEnumerable<string> _filePaths;
    private readonly IFileReader _fileReader;

    public LineFileProducer(ILogger<LineFileProducer> logger, IWordStorage wordStorage, IFileReader fileReader, IEnumerable<string> filePaths)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _producerId = ShortId.Generate();
      _filePaths = filePaths ?? throw new ArgumentNullException(nameof(filePaths));
      _fileReader = fileReader ?? throw new ArgumentNullException(nameof(fileReader));
    }

    public async Task ProduceAsync(CancellationToken cancellationToken = default)
    {
      foreach (var filePath in _filePaths)
      {
        _logger.LogInformation("Producer '{producerId}' Started to publish form file '{filePath}'", _producerId, filePath);
        await _fileReader.ReadFile(filePath).ConfigureAwait(false);
        _logger.LogInformation("Producer '{producerId}' finished to publish", _producerId);
      }
    }
  }
}
