using Microsoft.Extensions.Logging;
using WordCounterLibrary.LineToWords;

namespace WordCounterLibrary.Services
{
  internal class LineFileProducerService : ILineFileProducerService
  {
    private readonly ILogger<LineFileProducer> _logger;
    private readonly IBufferWriter _bufferWriter;
    private readonly IFileReader _fileReader;

    public LineFileProducerService(ILogger<LineFileProducer> logger, IBufferStorage bufferStorage, IFileReader fileReader)
    {
      if (bufferStorage is null) { throw new ArgumentNullException(nameof(bufferStorage)); }

      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _bufferWriter = bufferStorage.Writer ?? throw new ArgumentNullException(nameof(bufferStorage.Writer));
      _fileReader = fileReader ?? throw new ArgumentNullException(nameof(fileReader));
    }

    public ILineFileProducer Creator()
    {
      return new LineFileProducer(_logger, _bufferWriter, _fileReader);
    }
  }
}
