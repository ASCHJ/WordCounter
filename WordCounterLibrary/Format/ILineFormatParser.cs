namespace WordCounterLibrary.Format
{
  internal interface ILineFormatParser
  {
    IEnumerable<KeyValuePair<string, int>> GetWordKeyPairs(string line);
  }
}
