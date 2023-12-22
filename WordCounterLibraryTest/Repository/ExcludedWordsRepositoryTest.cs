using WordCounterLibrary.Repository;
using Xunit;

namespace WordCounterLibraryTest.Repository
{
  public class ExcludedWordsRepositoryTest
  {
    [Fact]
    public void Add_WhenAddingOneWord_ThenCountIsOne()
    {
      // Arrange
      var repository = new ExcludedWordsRepository();

      // Act
      repository.Add("word");

      // Assert
      Assert.Equal(1, repository.Count);
    }

    [Fact]
    public void Add_WhenAddingTwoWord_ThenCountIsTwo()
    {
      // Arrange
      var repository = new ExcludedWordsRepository();

      // Act
      repository.Add("word1");
      repository.Add("word2");

      // Assert
      Assert.Equal(2, repository.Count);
    }

    [Fact]
    public void Add_WhenAddingTheSameWordTwice_ThenCountIsOne()
    {
      // Arrange
      var repository = new ExcludedWordsRepository();

      // Act
      repository.Add("word");
      repository.Add("word");

      // Assert
      Assert.Equal(1, repository.Count);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Add_WhenWordIsNullOrEmpty_ThenCountIsUnchanged(string? input)
    {
      // Arrange
      var repository = new ExcludedWordsRepository();

      // Act
      repository.Add(input!);

      // Assert
      Assert.Equal(0, repository.Count);
    }

    [Fact]
    public void GetExcludedWords_WhenAddingOneWord_ThenAListContaingOneWordIsReturned()
    {
      // Arrange
      var repository = new ExcludedWordsRepository();

      // Act
      repository.Add("word1");

      // Assert
      Assert.Equal(new[] { "word1" }, repository.GetExcludedWords());
    }

    [Fact]
    public void GetExcludedWords_WhenAddingMultipleWords_ThenMultipleWordsAreReturned()
    {
      // Arrange
      var repository = new ExcludedWordsRepository();

      // Act
      repository.Add("word1");
      repository.Add("word2");

      // Assert
      Assert.Equal(new[] { "word1", "word2" }, repository.GetExcludedWords());
    }
  }
}
