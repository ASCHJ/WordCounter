namespace WordCounterLibrary.Repository
{
  internal class ExcludedWordsRepository : IExcludedWordsRepository
  {
    private readonly HashSet<string> _excludedWords = new();

    public void Add(string excludeWord)
    {
      if (!string.IsNullOrWhiteSpace(excludeWord))
      {
        _excludedWords.Add(excludeWord);
      }
    }

    public int Count => _excludedWords.Count;
    
    public IEnumerable<string> GetExcludedWords()
    {
      return _excludedWords.ToArray();
    }

    public bool IsExcludedWord(string word)
    {
      return _excludedWords.Contains(word, StringComparer.OrdinalIgnoreCase);
    }
  }
}
