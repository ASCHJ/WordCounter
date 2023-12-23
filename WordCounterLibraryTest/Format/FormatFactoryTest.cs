using WordCounterLibrary.Format;
using Xunit;

namespace WordCounterLibraryTest.Format
{
  public class FormatFactoryTest
  {
    [Fact]
    public void CreateFormat_WhenTIsWordAndCountFormat_ThenReturnWordAndCountFormat()
    {
      // Arrange
      var formatFactory = new FormatFactory();

      // Act
      var result = formatFactory.CreateFormat<WordAndCountFormat>();

      // Assert
      Assert.IsType<WordAndCountFormat>(result);
    }

    [Fact]
    public void CreateFormat_WhenTIsReportFormat_ThenReturnReportFormat()
    {
      // Arrange
      var formatFactory = new FormatFactory();

      // Act
      var result = formatFactory.CreateFormat<ReportFormat>();

      // Assert
      Assert.IsType<ReportFormat>(result);
    }

    [Fact]
    public void CreateFormat_WhenDefaultReport_ThenReturnReportFormat()
    {
      // Arrange
      var formatFactory = new FormatFactory();

      // Act
      var result = formatFactory.CreateFormat<SomeFormat>();

      // Assert
      Assert.IsType<ReportFormat>(result);
    }

    internal class SomeFormat : IFormatter
    {
      public void AppendLine(string word, int count)
      {     
      }

      public string GetContent()
      {
        return string.Empty;
      }
    }
  }
}
