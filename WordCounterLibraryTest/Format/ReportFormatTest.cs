using WordCounterLibrary.Format;
using Xunit;

namespace WordCounterLibraryTest.Format
{
  public class ReportFormatTest
  {
    [Fact]
    public void AppendLine_WhenWordAndCountIsAppended_ThenStringWithWordAndCountIsConstructed()
    {
      // Arrange
      var expectedOutput = "A 1" + Environment.NewLine;
      var reportFormat = new ReportFormat();

      // Act
      reportFormat.AppendLine("A", 1);

      // Assert
      
      Assert.Equal(expectedOutput, reportFormat.GetContent());
    }

    [Fact]
    public void AppendLine_WhenMultipleWordsAndCountsAreAppended_ThenStringWithMultipleWordsAndCountsIsConstructed()
    {
      // Arrange
      var expectedOutput = "A 1" + Environment.NewLine +
                           "B 2" + Environment.NewLine;

      var reportFormat = new ReportFormat();

      // Act
      reportFormat.AppendLine("A", 1);
      reportFormat.AppendLine("B", 2);

      // Assert
      Assert.Equal(expectedOutput, reportFormat.GetContent());
    }

    [Fact]
    public void AppendLine_WhenWordIsEmpty_ThenReturnStringIsEmpty()
    {
      // Arrange
      var line = string.Empty;
      var reportFormat = new ReportFormat();

      // Act
      reportFormat.AppendLine(line, 1);

      // Assert
      Assert.Empty(reportFormat.GetContent());
    }
  }
}
