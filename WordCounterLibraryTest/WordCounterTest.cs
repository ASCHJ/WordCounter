using Microsoft.Extensions.Logging;
using NSubstitute;
using WordCounterLibrary;
using WordCounterLibrary.LineToWords;
using WordCounterLibrary.WordsWriter;
using WordCounterLibraryTest.TestHelpers;
using Xunit;

namespace WordCounterLibraryTest
{
  public class WordCounterTests
  {
    [Fact]
    public async Task StartAsync_WhenNoFilesInDirectory_ThenNoFilesAreProcessed()
    {
      // Arrange
      var loggerMock = Substitute.For<ILogger<WordCounter>>();
      var lineManagerMock = Substitute.For<IWordsProcessor>();
      var reporterMock = Substitute.For<IReporter>();

      var wordCounter = new WordCounter(loggerMock, lineManagerMock, reporterMock);

      // Act
      await wordCounter.StartAsync("ANoneExistingDirectory", default);

      // Assert
      await lineManagerMock.Received(0).ExecuteAsync(Arg.Any<ushort>(), Arg.Any<ushort>(), Arg.Any<string[]>(), Arg.Any<CancellationToken>());
      reporterMock.Received(0).WriteReports();
    }

    [Fact]
    public async Task StartAsync_WhenFilesInDirectory_ThenProcessFiles() //TODO Will be fix when WordCounter is fixed
    {
      Assert.Fail("fix");
      // Arrange
      var loggerMock = Substitute.For<ILogger<WordCounter>>();
      var lineManagerMock = Substitute.For<IWordsProcessor>();
      var reporterMock = Substitute.For<IReporter>();

      lineManagerMock.ExecuteAsync(Arg.Any<ushort>(), Arg.Any<ushort>(), Arg.Any<string[]>(), Arg.Any<CancellationToken>())
          .Returns(Task.CompletedTask);

      var wordCounter = new WordCounter(loggerMock, lineManagerMock, reporterMock);
      var pathWithFiles = LocationHelper.GetDirectory(@"Data\");

      // Act
      await wordCounter.StartAsync(pathWithFiles, default);

      // Assert
      await lineManagerMock.Received(1).ExecuteAsync(Arg.Any<ushort>(), Arg.Any<ushort>(), Arg.Any<string[]>(), Arg.Any<CancellationToken>());
      reporterMock.Received(1).WriteReports();
    }
  }
}

