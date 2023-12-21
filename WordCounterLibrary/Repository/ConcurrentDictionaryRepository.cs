using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace WordCounterLibrary.Repository
{
  public class ConcurrentDictionaryRepository : IWordRepository
  {
    private const int _addOrUpdateRetries = 3;

    private readonly ConcurrentDictionary<string, int> _wordStorage = new(StringComparer.OrdinalIgnoreCase);
    private readonly ILogger<ConcurrentDictionaryRepository> _logger;

    public ConcurrentDictionaryRepository(ILogger<ConcurrentDictionaryRepository> logger)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public int Count => _wordStorage.Count;

    public void AddOrUpdate(string word, int wordCount)
    {
      int attempt = 1;
      while (attempt <= _addOrUpdateRetries)
      {
        bool methodResult = RetrieveAndUpdateOrAdd(word, wordCount);

        if (methodResult)
        {
          return;
        }
        _logger.LogDebug("Attempt {attempt} failed. Retrying...", attempt);
        attempt++;
      }

      _logger.LogError("Error after {attempt} tries.", attempt - 1);
      throw new Exception($"Error after {attempt - 1} tries"); //TODO handler better - custom exception
    }

    public KeyValuePair<string, int> ElementAtOrDefault(int index)
    {
      return _wordStorage.ElementAtOrDefault(index);
    }

    /// <summary>
    /// OBS: Since the pair values need to be modified it can fail on add and update, due to multiable threads accessing the same pair key / value
    /// Source: https://learn.microsoft.com/en-us/dotnet/standard/collections/thread-safe/how-to-add-and-remove-items
    /// </summary>
    private bool RetrieveAndUpdateOrAdd(string word, int wordCount)
    {
      if (_wordStorage.TryGetValue(word, out int retrievedValue))
      {
        var count = retrievedValue + wordCount;
        if (_wordStorage.TryUpdate(word, count, retrievedValue))
        {
          _logger.LogDebug("Updated {word} count to {count}", word, count);
          return true;
        }
        else
        {
          _logger.LogWarning("Could not update '{word}' with count {count}", word, count);
          return false;
        }
      }
      else
      {
        var count = wordCount;
        if (_wordStorage.TryAdd(word.ToUpper(), count))
        {
          _logger.LogDebug("Added {word} with count {newValue}", word, count);
          return true;
        }
        else
        {
          _logger.LogWarning("Unable to add '{word}' with count {count}", word, count);
          return false;
        }
      }
    }
  }
}
