using Microsoft.Extensions.Logging;
using NSubstitute;
using WordCounterLibrary;
using WordCounterLibrary.LineToWords;
using WordCounterLibrary.Managers;
using WordCounterLibrary.WordsWriter;
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
      var iOManagerMock = Substitute.For<IIOManager>();
      var lineManagerMock = Substitute.For<IWordsProcessor>();
      var reporterMock = Substitute.For<IReporter>();

      var wordCounter = new WordCounter(loggerMock, iOManagerMock, lineManagerMock, reporterMock);

      // Act
      iOManagerMock.GetFilesInDirectory(Arg.Any<string>(), Arg.Any<string>()).Returns(Array.Empty<string>());
      await wordCounter.StartAsync("ANoneExistingDirectory", default);

      // Assert
      await lineManagerMock.Received(0).ExecuteAsync(Arg.Any<ushort>(), Arg.Any<ushort>(), Arg.Any<string[]>(), Arg.Any<CancellationToken>());
      reporterMock.Received(0).WriteReports();
    }

    [Fact]
    public async Task StartAsync_WhenFilesInDirectory_ThenProcessFiles()
    {
      // Arrange
      var loggerMock = Substitute.For<ILogger<WordCounter>>();
      var iOManagerMock = Substitute.For<IIOManager>();
      var lineManagerMock = Substitute.For<IWordsProcessor>();
      var reporterMock = Substitute.For<IReporter>();

      lineManagerMock.ExecuteAsync(Arg.Any<ushort>(), Arg.Any<ushort>(), Arg.Any<string[]>(), Arg.Any<CancellationToken>())
          .Returns(ExecutionStatus.ExecutionCompleted);

      var wordCounter = new WordCounter(loggerMock, iOManagerMock, lineManagerMock, reporterMock);

      // Act
      iOManagerMock.GetFilesInDirectory(Arg.Any<string>(), Arg.Any<string>()).Returns(new List<string> { @"c:\someexisting\path" }.ToArray());
      await wordCounter.StartAsync("ExistingDirectory", default);

      // Assert
      await lineManagerMock.Received(1).ExecuteAsync(Arg.Any<ushort>(), Arg.Any<ushort>(), Arg.Any<string[]>(), Arg.Any<CancellationToken>());
      reporterMock.Received(1).WriteReports();
    }
  }
}

