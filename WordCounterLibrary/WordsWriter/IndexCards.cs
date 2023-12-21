using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace WordCounterLibrary.WordsWriter
{
  internal class IndexCards : IIndexCards
  {
    private readonly ConcurrentDictionary<char, HashSet<int>> _indexCards = new();
    private readonly ConcurrentDictionary<int, int> _excludedIndexes = new();
    private readonly ILogger<IndexCards> logger;

    public IndexCards(ILogger<IndexCards> logger)
    {
      this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void CreateIndexKey(char character)
    {
      _indexCards.TryAdd(character, new HashSet<int>());
    }

    public void Add(string word, int index)
    {
      if (string.IsNullOrEmpty(word)) { throw new ArgumentException($"'{nameof(word)}' cannot be null or empty.", nameof(word)); }

      char characterUpper = char.ToUpper(word[0]);
      if (_indexCards.TryGetValue(characterUpper, out var indexList))
      {
        indexList.Add(index);
        return;
      }

      logger.LogWarning("No index cards for word {word} with index {index}", word, index);
    }

    public IEnumerable<KeyValuePair<char, HashSet<int>>> GetIndexCards()
    {
      return _indexCards;
    }

    public void AddExcludedIndex(int index)
    {
      _excludedIndexes.AddOrUpdate(index, 0, (key, oldValue) => 0); //Only 'key' is used, so value does not matter
    }

    public IEnumerable<int> GetExcludedIndexCards()
    {
      return _excludedIndexes.Keys;
    }
  }
}
