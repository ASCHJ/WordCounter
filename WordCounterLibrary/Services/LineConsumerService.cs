using Microsoft.Extensions.Logging;
using WordCounterLibrary.Format;
using WordCounterLibrary.LineToWords;
using WordCounterLibrary.Repository;

namespace WordCounterLibrary.Services
{
  internal class LineConsumerService : ILineConsumerService
  {
    private readonly ILogger<LineConsumer> _logger;
    private readonly IBufferReader _reader;
    private readonly ILineFormatParser _lineFormatParser;
    private readonly IWordRepository _wordStorage;

    public LineConsumerService(ILogger<LineConsumer> logger, IBufferStorage bufferStorage, ILineFormatParser lineFormatParser, IWordRepository wordStorage)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _reader = bufferStorage?.Reader ?? throw new ArgumentNullException(nameof(bufferStorage));
      _lineFormatParser = lineFormatParser ?? throw new ArgumentNullException(nameof(lineFormatParser));
      _wordStorage = wordStorage ?? throw new ArgumentNullException(nameof(wordStorage));
    }

    public ILineConsumer Creator()
    {
      return new LineConsumer(_logger, _reader, _lineFormatParser, _wordStorage);
    }
  }
}
