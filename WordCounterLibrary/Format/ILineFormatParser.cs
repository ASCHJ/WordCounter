namespace WordCounterLibrary.Format
{
  internal interface ILineFormatParser
  {
    string[] GetWords(string inputString);
  }
}