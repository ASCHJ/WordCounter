namespace WordCounterLibrary.IO
{
  internal class StreamFileReader : IFileReader
  {
    private readonly StreamReader _streamReader;

    public StreamFileReader(string path)
    {
      if (string.IsNullOrWhiteSpace(path)) { throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path)); }

      var sourceStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
      _streamReader = new StreamReader(sourceStream);
    }

    public async Task<string?> ReadLineAsync() => await _streamReader.ReadLineAsync();

    public bool EndOfStream => _streamReader.EndOfStream;

    public void Dispose() => _streamReader.Dispose();
  }
}
