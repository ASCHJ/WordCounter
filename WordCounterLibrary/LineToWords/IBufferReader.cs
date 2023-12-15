namespace WordCounterLibrary.LineToWords
{
  internal interface IBufferReader
  {
    Task Completion { get; }
    IAsyncEnumerable<string> ReadAllAsync(CancellationToken cancellationToken);
  }
}
