using Microsoft.Extensions.Logging;
using shortid;
using System.Buffers;
using WordCounterLibrary.Services;

namespace WordCounterLibrary.LineToWords
{
  internal class LineFileProducer : ILineFileProducer
  {
    private ILogger<LineFileProducer> _logger;
    private readonly IBufferWriter _bufferWriter;
    private readonly string _producerId;
    private readonly IFileReaderService _fileReaderService;

    public LineFileProducer(ILogger<LineFileProducer> logger, IBufferWriter bufferWriter, IFileReaderService fileReaderService)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _bufferWriter = bufferWriter ?? throw new ArgumentNullException(nameof(bufferWriter));
      _producerId = ShortId.Generate();
      _fileReaderService = fileReaderService ?? throw new ArgumentNullException(nameof(fileReaderService));
    }

    public async Task ProduceAsync(IEnumerable<string> filePaths, CancellationToken cancellationToken = default)
    {
      foreach (var filePath in filePaths)
      {
        _logger.LogInformation("Producer '{producerId}' Started to publish from file '{filePath}'", _producerId, filePath);

        using (var reader = _fileReaderService.GetReader(filePath))
        {
          while (!reader.EndOfStream)
          {
            var line = await reader.ReadLineAsync();
            if (line != null) { 
              await _bufferWriter.WriteAsync(line, CancellationToken.None);
            }
          }
        }
        _logger.LogInformation("Producer '{producerId}' finished to publish", _producerId);
      }
    }
  }
}
