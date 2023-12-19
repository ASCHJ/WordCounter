using WordCounterLibrary.WordsWriter;
using Xunit;

namespace WordCounterLibraryTest.WordsWriter
{
  public class AsciiAlphabetTest
  {
    [Fact]
    public void Get_WhenAlphabetIsUppercase_ThenItMatchesAllUpperCaseASCIICharactures()
    {
      // Arrange
      string expectedString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
      var alphabet = new AsciiUpperCaseAlphabet();

      // Act
      var asciiUpperCaseAlphabet = alphabet.Get();

      // Assert
      Assert.Equal(expectedString.Length, asciiUpperCaseAlphabet.Count());
      foreach (var expectedChar in expectedString)
      {
        Assert.Contains(expectedChar, asciiUpperCaseAlphabet);
      }
    }
  }
}
