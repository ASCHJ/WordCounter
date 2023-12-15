namespace WordCounterLibrary.LineToWords
{
  internal interface IFileReader
  {
    Task ReadFile(string filePath);
  }
}
