namespace WordCounterLibrary.IO
{
  internal class StreamFileWriter : IFileWriter
  {
    public void Write(string path, string content)
    {
      using StreamWriter outputFile = new(path);
      outputFile.Write(content);
    }
  }
}
