using WordCounterLibrary.IO;
using WordCounterLibrary.Services;
using Xunit;

namespace WordCounterLibraryTest.Services
{
  public class FileReaderServiceTest
  {
    [Fact]
    public void Creator_WhenCreatorIsCalled_ThenReturnsConsumer()
    {
      // Arrange
      var fileReaderService = new FileReaderService();

      // Act
      var fileReader = fileReaderService.GetReader(SomeExistingFileInCurrentFolder());

      // Assert
      Assert.NotNull(fileReader);
      Assert.IsAssignableFrom<IFileReader>(fileReader);
    }

    private static string SomeExistingFileInCurrentFolder()
    {
      return Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory).First();
    }
  }
}
