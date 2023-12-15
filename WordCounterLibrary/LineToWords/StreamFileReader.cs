using Microsoft.Extensions.Logging;
using System.Text;
using WordCounterLibrary.Format;

namespace WordCounterLibrary.LineToWords
{
  internal class StreamFileReader : IFileReader
  {
    private const int BufferSize = 4096;

    private readonly ILogger<StreamFileReader> _logger;

    private readonly IWordStorage _wordStorage;
    private readonly ILineFormatParser _lineFormatParser;

    public StreamFileReader(ILogger<StreamFileReader> logger, IWordStorage wordStorage, ILineFormatParser lineFormatParser)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _wordStorage = wordStorage ?? throw new ArgumentNullException(nameof(wordStorage));
      _lineFormatParser = lineFormatParser ?? throw new ArgumentNullException(nameof(lineFormatParser));
    }

    public async Task ReadFile(string filePath)
    {
      try
      {
        using FileStream sourceStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: BufferSize, useAsync: true);
        using StreamReader reader = new(sourceStream);

        string? line = null;
        while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
        {
          var words = _lineFormatParser.GetWords(line);
          foreach (var word in words)
          {
            _wordStorage.AddOrUpdate(word, 1);
          }
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"Error reading file: {filePath}");
        throw; // TODO: do more?
      }
    }
  }
}
