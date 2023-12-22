using System.IO;

namespace WordCounterLibrary.IO
{
  public interface IFileReader : IDisposable
  {
    Task<string?> ReadLineAsync();
    bool EndOfStream { get; }
  }
}
