namespace WordCounterLibrary.IO
{
  internal interface IFileWriter
  {
    void Write(string path, string content);
  }
}
