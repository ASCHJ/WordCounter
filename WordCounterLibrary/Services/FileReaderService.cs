using WordCounterLibrary.IO;

namespace WordCounterLibrary.Services
{
  internal class FileReaderService : IFileReaderService
  {
    public IFileReader GetReader(string path)
    {
      return new StreamFileReader(path);
    }
  }
}
