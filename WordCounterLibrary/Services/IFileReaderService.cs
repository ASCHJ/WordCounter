using WordCounterLibrary.IO;

namespace WordCounterLibrary.Services
{
  internal interface IFileReaderService
  {
    IFileReader GetReader(string path);
  }
}
