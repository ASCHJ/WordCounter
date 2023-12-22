using Microsoft.Extensions.Logging;
using WordCounterLibrary.LineToWords;

namespace WordCounterLibrary.Services
{
  internal class LineFileProducerService : ILineFileProducerService
  {
    private readonly ILogger<LineFileProducer> _logger;
    private readonly IBufferWriter _bufferWriter;
    private readonly IFileReaderService _fileReaderService;

    public LineFileProducerService(ILogger<LineFileProducer> logger, IBufferStorage bufferStorage, IFileReaderService fileReaderService)
    {
      if (bufferStorage is null) { throw new ArgumentNullException(nameof(bufferStorage)); }

      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _bufferWriter = bufferStorage.Writer ?? throw new ArgumentNullException(nameof(bufferStorage.Writer));
      _fileReaderService = fileReaderService ?? throw new ArgumentNullException(nameof(fileReaderService));
    }

    public ILineFileProducer Creator()
    {
      return new LineFileProducer(_logger, _bufferWriter, _fileReaderService);
    }
  }
}
