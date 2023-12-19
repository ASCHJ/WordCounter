using WordCounterLibrary.Format;
using Xunit;

namespace WordCounterLibraryTest.Format
{
  public class LipsumLineFormatParserTest
  {
    [Fact]
    public void GetWordKeyPairs_WhenLineInputIsNull_ThenThrowArgumentNullException()
    {
      // Arrange
      string? value = null;
      var parser = new LipsumLineFormatParser();

      // Act & Assert
      Assert.Throws<ArgumentNullException>(() => parser.GetWordKeyPairs(value!));
    }

    [Theory]
    [InlineData("a b c", 3)]
    [InlineData("a a c", 2)]
    [InlineData("a a a", 1)]
    public void GetWordKeyPairs_WhenLinesWithWords_ThenReturnExpectedResult(string line, int expectWordCount)
    {
      // Arrange
      var parser = new LipsumLineFormatParser();

      // Act
      var result = parser.GetWordKeyPairs(line);

      // Assert
      Assert.Equal(expectWordCount, result.Count());
    }
    
    [Theory]
    [InlineData("a.b.c.")]
    [InlineData(".a.b.c.")]
    [InlineData("..a..b..c..")]
    [InlineData("a, b, c,")]
    [InlineData(",a,, b,, c,,")]
    [InlineData("a  b   c  ")]
    [InlineData(" a  b   c  ")]
    public void GetWordKeyPairs_WhenLinesWithSeparators_ThenReturnExpectedResult(string line)
    {
      // Arrange
      var parser = new LipsumLineFormatParser();

      // Act
      var result = parser.GetWordKeyPairs(line);

      // Assert
      Assert.Equal(3, result.Count());
    }
  }
}
