using WordCounterLibrary.Format;
using WordCounterLibraryTest.TestHelpers;
using Xunit;

namespace WordCounterLibraryTest.Format
{
  public class LineFormatParserIntegrationTest
  {
    [Theory]
    [InlineData("Aenean efficitur quis diam eu tristique. Duis neque ipsum, tristique id auctor eu, sagittis ut lorem. Curabitur eget metus tincidunt, semper urna eget, feugiat nibh. Quisque.", 26)]
    [InlineData("Nullam eu massa at velit dictum ultricies.Aliquam erat volutpat.Proin fringilla aliquet turpis.Nam egestas orci sit amet eros convallis pulvinar.Phasellus aliquet mattis augue, id feugiat elit dictum vel.Phasellus vehicula orci non orci posuere, ut sollicitudin massa feugiat. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Donec varius, orci eu efficitur pulvinar, metus turpis iaculis neque, vestibulum rhoncus urna enim nec nulla.Duis leo turpis, congue ac odio eu, pretium fringilla orci.Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas.", 93)]
    [InlineData("Vestibulum lectus diam, gravida id sem at, porttitor finibus velit. Nullam fringilla tempus nulla eget bibendum. Phasellus nulla sapien, porta et justo quis, semper pellentesque mi. Fusce scelerisque ligula in porta varius. Nam eget dui a mi laoreet scelerisque et a eros. In sit amet sapien eu nulla ultrices pretium. Nullam efficitur quis massa non tincidunt. Donec tempus ultricies sapien nec dignissim.", 62)]
    public void LipsumLineFormatParser_ShouldReturnCorrectWordCount_ForTestFileSample(string input, int expectedWords)
    {
      // Arrange
      var parser = new LipsumLineFormatParser();

      // Act
      var words = parser.GetWords(input);

      // Assert
      Assert.Equal(expectedWords, words.Count());
    }

    [Theory]
    [InlineData("200.txt", 200)]
    [InlineData("300.txt", 300)]
    [InlineData("400.txt", 400)]
    [InlineData("500.txt", 500)]
    public void LipsumLineFormatParser_ShouldReturnCorrectWordCount_ForTestGeneratedFileSample(string filename, int expectedWords)
    {
      // Arrange
      var pathToFile = GetFilePath(filename);
      string[] lines = File.ReadAllLines(pathToFile);
      var parser = new LipsumLineFormatParser();

      // Act
      var wordCount = lines.Sum(line => parser.GetWords(line).Length);

      // Assert
      Assert.Equal(expectedWords, wordCount);
    }

    private string GetFilePath(string filename)
    {
      return Path.Combine(LocationHelper.CurrentDirectory(@"Data\"), filename);
    }
  }
}
