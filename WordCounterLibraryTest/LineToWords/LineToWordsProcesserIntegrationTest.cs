using Microsoft.Extensions.Logging;
using System.Diagnostics;
using WordCounterLibrary.LineToWords;
using WordCounterLibraryTest.TestHelpers;
using Xunit;
using Xunit.Abstractions;

namespace WordCounterLibraryTest.LineToWords
{
  public class LineToWordsProcesserIntegrationTest : IDisposable
  {
    private ILoggerFactory LoggerFactory { get; }
    private ITestOutputHelper TestOutputHelper { get; }

    public LineToWordsProcesserIntegrationTest(ITestOutputHelper outputHelper)
    {
      TestOutputHelper = outputHelper;
      LoggerFactory = outputHelper.BuildLoggerFactory(LogLevel.Debug);
    }

    [Fact]
    public async Task LineToWordsProcessor_ReadAllFilesInTestDirectory_KnownExpectedOutCome()
    {
      // Arrange
      var expectedWordsInTotal = 1400;

      int producersCount = 4;
      int consumersCount = 1;
      var memoryStorage = new MemoryStorage(LoggerFactory.CreateLogger<MemoryStorage>());
      var lineManager = new LineToWordsProcessor(LoggerFactory, memoryStorage);

      var dataFolder = LocationHelper.CurrentDirectory(@"Data\");
      string[] filesInDir = Directory.GetFiles(dataFolder, "*.txt");

      var cancellationTokeSource = new CancellationTokenSource();
      var cancellationToken = cancellationTokeSource.Token;

      // Act
      var exception = await Record.ExceptionAsync(() => lineManager.Execute(producersCount, consumersCount, filesInDir, cancellationToken));
      Assert.Null(exception);

      // Assert
      int sum = memoryStorage.WordCount;
      Assert.Equal(expectedWordsInTotal, sum);
    }

    [Theory]
    [InlineData("200.txt", 200, 1, 5)]
    [InlineData("300.txt", 300, 1, 5)]
    [InlineData("400.txt", 400, 1, 5)]
    [InlineData("500.txt", 500, 1, 5)]
    [InlineData("200.txt", 200, 3, 10)]
    [InlineData("300.txt", 300, 3, 10)]
    [InlineData("400.txt", 400, 3, 10)]
    [InlineData("500.txt", 500, 3, 10)]
    [InlineData("200.txt", 200, 6, 10)]
    [InlineData("300.txt", 300, 6, 10)]
    [InlineData("400.txt", 400, 6, 10)]
    [InlineData("500.txt", 500, 6, 10)]
    public async Task LineToWordsProcesser_DiffirentTestFilesWithDifferentProducersAndConsumer_ExpectedWordWound(string filename, int expectedWords, int producersCount, int consumersCount)
    {
      // Arrange
      var memoryStorage = new MemoryStorage(LoggerFactory.CreateLogger<MemoryStorage>());
      var lineToWordsProcessor = new LineToWordsProcessor(LoggerFactory, memoryStorage);

      var pathToFile = Path.Combine(LocationHelper.CurrentDirectory(@"Data\"), filename);
      string[] filesInDir = new string[] { pathToFile };

      var cancellationTokeSource = new CancellationTokenSource();
      var cancellationToken = cancellationTokeSource.Token;

      // Act
      Stopwatch stopwatch = Stopwatch.StartNew();
      var exception = await Record.ExceptionAsync(() => lineToWordsProcessor.Execute(producersCount, consumersCount, filesInDir, cancellationToken));
      stopwatch.Stop();

      // Assert
      Assert.Null(exception);

      TestOutputHelper.WriteLine($"Execution time: {stopwatch.ElapsedMilliseconds} milliseconds");
      Assert.Equal(expectedWords, memoryStorage.WordCount);
    }

    public void Dispose()
    {
      LoggerFactory?.Dispose();
    }
  }
}
