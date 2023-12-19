using WordCounterLibrary.Format;
using Xunit;

namespace WordCounterLibraryTest.Format
{
  public class WordAndCountFormatTest
  {
    [Fact]
    public void AppendLine_WordAndCount_ConstructsStringWithWordAndCount()
    {
      // Arrange
      var expectedOutput = "  ABC 1" + Environment.NewLine;
      var wordAndCountFormat = new WordAndCountFormat();

      // Act
      wordAndCountFormat.AppendLine("ABC", 1);

      // Assert
      Assert.Equal(expectedOutput, wordAndCountFormat.ToString());
    }

    [Theory]
    [InlineData("Abc")]
    [InlineData("aBc")]
    [InlineData("abC")]
    [InlineData("abc")]
    [InlineData("ABC")]
    public void AppendLine_WhenDifferentWordsAreUsedAsInput_ThenReturnsUpperCasedWord(string word)
    {
      // Arrange
      var expectedOutput = "  ABC 1" + Environment.NewLine;
      var wordAndCountFormat = new WordAndCountFormat();

      // Act
      wordAndCountFormat.AppendLine(word, 1);

      // Assert
      Assert.Equal(expectedOutput, wordAndCountFormat.ToString());
    }

    [Fact]
    public void AppendLine_WhenWordIsEmpty_ThenReturnsEmptyString()
    {
      // Arrange
      var word = string.Empty;
      var wordAndCountFormat = new WordAndCountFormat();

      // Act
      wordAndCountFormat.AppendLine(word, 1);
      
      // & Assert
      Assert.Empty(wordAndCountFormat.ToString());
    }
  }
}
