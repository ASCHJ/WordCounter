using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using WordCounterLibrary.Repository;
using Xunit;

namespace WordCounterLibraryTest.Repository
{
  public class ConcurrentDictionaryRepositoryTest
  {
    readonly ILogger<ConcurrentDictionaryRepository> _logger = new NullLogger<ConcurrentDictionaryRepository>();

    [Fact]
    public void WordCount_WhenInilializedWithNullLogger_ThenThrowArgumentNullException()
    {
      // Act / Assert
      Assert.Throws<ArgumentNullException>(() => new ConcurrentDictionaryRepository(null!));
    }

    [Fact]
    public void WordCount_WhenInilialized_ThenRepositoryIsEmpty()
    {
      // Arrange
      var repository = new ConcurrentDictionaryRepository(_logger);

      // Act
      var result = repository.Count;

      // Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void WordCount_WhenWordIsAdded_ThenReturnCount()
    {
      // Arrange
      var repository = new ConcurrentDictionaryRepository(_logger);
      repository.AddOrUpdate("a", 1);

      // Act
      var result = repository.Count;

      // Assert
      Assert.Equal(1, result);
    }

    [Fact]
    public void WordCount_WhenMultipleUniqueWordsIsAdded_ThenReturnsCountOfWords()
    {
      // Arrange
      var repository = new ConcurrentDictionaryRepository(_logger);
      var word1 = "a";
      var count1 = 1;
      var word2 = "b";
      var count2 = 2;

      repository.AddOrUpdate(word1, count1);
      repository.AddOrUpdate(word2, count2);

      // Act
      var result = repository.Count;

      // Assert
      Assert.Equal(2, result);
    }

    [Fact]
    public void WordCount_WhenMultipleOfTheSameWordIsAdded_ThenReturnsCountOfOne()
    {
      // Arrange
      var repository = new ConcurrentDictionaryRepository(_logger);
      var word1 = "a";
      var count1 = 1;
      var word2 = "a";
      var count2 = 2;

      repository.AddOrUpdate(word1, count1);
      repository.AddOrUpdate(word2, count2);

      // Act
      var result = repository.Count;

      // Assert
      Assert.Equal(1, result);
    }

    [Fact]
    public void AddOrUpdate_WhenAddingSameWord_ThenCountIsUpdated()
    {
      // Arrange
      var repository = new ConcurrentDictionaryRepository(_logger);
      var word = "A";
      var count = 2;
      var addCount = 3;

      // Act
      repository.AddOrUpdate(word, count);
      repository.AddOrUpdate(word, addCount);

      // Assert
      var result = repository.ElementAtOrDefault(0);
      Assert.Equal(word, result.Key);
      Assert.Equal(count + addCount, result.Value);
    }

    [Fact]
    public void AddOrUpdate_WhenWordIsUpperAndLowerCased_ThenUpperCasedIsAddedForBoth()
    {
      // Arrange
      var repository = new ConcurrentDictionaryRepository(_logger);
      var wordWithSmallLetter = "a";
      var count1 = 1;
      var wordWithBigLetter = "A";
      var count2 = 2;

      // Act
      repository.AddOrUpdate(wordWithSmallLetter, count1);
      repository.AddOrUpdate(wordWithBigLetter, count2);

      // Assert
      var result = repository.ElementAtOrDefault(0);
      Assert.Equal("A", result.Key);
      Assert.Equal(count1 + count2, result.Value);
    }
  }
}
