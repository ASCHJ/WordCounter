using Microsoft.Extensions.Logging;

namespace WordCounterLibrary.LineToWords
{
  internal class StreamFileReader : IFileReader
  {
    private const int BufferSize = 4096;

    private readonly ILogger<StreamFileReader> _logger;
    private readonly IBufferWriter _bufferWriter;

    public StreamFileReader(ILogger<StreamFileReader> logger, IBufferStorage bufferStorage)
    {
      if (bufferStorage is null) { throw new ArgumentNullException(nameof(bufferStorage)); }

      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _bufferWriter = bufferStorage.Writer;
    }

    public async Task WriteFileContentToBufferAsync(string filePath)
    {
      try
      {
        using FileStream sourceStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: BufferSize, useAsync: true);
        using StreamReader reader = new(sourceStream);

        string? line = null;
        while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
        {
          await _bufferWriter.WriteAsync(line, CancellationToken.None);
        }
      }
      catch (Exception ex)
      {
        var message = $"Error reading file: {filePath}";
        _logger.LogError(ex, message);
        throw;
      }
    }

    public async Task<IReadOnlyList<string>> ReadFileContent(string filePath)
    {
      var lines = new List<string>();
      try
      {
        using FileStream sourceStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: BufferSize, useAsync: true);
        using StreamReader reader = new(sourceStream);

        string? line = null;
        while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) != null)
        {
          lines.Add(line);
        }
      }
      catch (Exception ex)
      {
        var message = $"Error reading file: {filePath}";
        _logger.LogError(ex, message);
        throw;
      }

      return lines;
    }
  }
}
