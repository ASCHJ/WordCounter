namespace WordCounterLibrary.LineToWords
{
  internal interface IBufferWriter
  {
    void Complete();
    Task WriteAsync(string line, CancellationToken cancellationToken);
  }
}