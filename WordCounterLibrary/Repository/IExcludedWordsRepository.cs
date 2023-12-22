namespace WordCounterLibrary.Repository
{
  internal interface IExcludedWordsRepository
  {
    void Add(string excludeWord);
    int Count { get; }
    bool IsExcludedWord(string word);
    IEnumerable<string> GetExcludedWords();
  }
}
