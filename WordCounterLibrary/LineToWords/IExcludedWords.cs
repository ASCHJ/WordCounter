
namespace WordCounterLibrary.LineToWords
{
  internal interface IExcludedWords
  {
    Task ReadExcludedWords(string path);
    bool IsExcludedWord(string word);
    IEnumerable<string> GetExcludedWords();
  }
}
