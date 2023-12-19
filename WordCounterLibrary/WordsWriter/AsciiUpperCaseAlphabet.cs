namespace WordCounterLibrary.WordsWriter
{
  internal class AsciiUpperCaseAlphabet : IAlphabet
  {
    public IEnumerable<char> Get()
    {
      char[] asciiAlphabet = new char[26];

      for (int i = 0; i < 26; i++)
      {
        asciiAlphabet[i] = (char)(65 + i);
      }

      return asciiAlphabet;
    }
  }
}
