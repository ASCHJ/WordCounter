using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using WordCounterLibrary.Helpers;

namespace WordCounterLibrary.LineToWords
{
  public class MemoryStorage : IWordStorage
  {
    private const int AddOrUpdateRetries = 3;

    private readonly ConcurrentDictionary<string, int> wordStorage = new();
    private readonly ILogger<MemoryStorage> _logger;
    private readonly IExecutor _executor;

    public MemoryStorage(ILogger<MemoryStorage> logger) : this(logger, new Executor(logger))
    {
    }

    public MemoryStorage(ILogger<MemoryStorage> logger, IExecutor executor)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _executor = executor ?? throw new ArgumentNullException(nameof(executor));
    }

    public int WordCount => WordCounter();

    private int WordCounter() //TODO: tempory method?
    {
      return wordStorage.Values.Sum();
    }

    public void AddOrUpdate(string word, int wordCount)
    {
      _executor.Retries(() => RetrieveAndUpdateOrAdd(word, wordCount), AddOrUpdateRetries);
    }

    public KeyValuePair<string, int>[] Snapshot()
    {
      return wordStorage.ToArray();
    }

    /// <summary>
    /// OBS: Since the pair values need to be modified it can fail on add and update, due to multiable threads accessing the same pair key / value
    /// Source: https://learn.microsoft.com/en-us/dotnet/standard/collections/thread-safe/how-to-add-and-remove-items
    /// </summary>
    private bool RetrieveAndUpdateOrAdd(string word, int wordCount)
    {
      if (wordStorage.TryGetValue(word, out int retrievedValue))
      {
        var count = retrievedValue + wordCount;
        if (wordStorage.TryUpdate(word, count, retrievedValue))
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
        if (wordStorage.TryAdd(word, count))
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
