using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using WordCounterLibrary.LineToWords;
using WordCounterLibrary.Services;
using Xunit;

namespace WordCounterLibraryTest.LineToWords
{
  public class LineToWordsProcesserTest
  {
    [Fact]
    public void Constructor_WhenLoggerIsNull_ThenThrowArgumentNullException()
    {
      // Arrange
      var bufferStorage = Substitute.For<IBufferStorage>();
      var lineFileProducerService = Substitute.For<ILineFileProducerService>();
      var lineConsumerService = Substitute.For<ILineConsumerService>();

      // Act / Assert
      Assert.Throws<ArgumentNullException>(() => new LineToWordsProcessor(null!, bufferStorage, lineFileProducerService, lineConsumerService));
    }

    [Fact]
    public void Constructor_WhenBufferStorageIsNull_ThenThrowArgumentNullException()
    {
      // Arrange
      var logger = Substitute.For<ILogger<LineToWordsProcessor>>();
      var lineFileProducerService = Substitute.For<ILineFileProducerService>();
      var lineConsumerService = Substitute.For<ILineConsumerService>();

      // Act / Assert
      Assert.Throws<ArgumentNullException>(() => new LineToWordsProcessor(logger, null!, lineFileProducerService, lineConsumerService));
    }

    [Fact]
    public void Constructor_WhenLineFileProducerServiceIsNull_ThenThrowArgumentNullException()
    {
      // Arrange
      var logger = Substitute.For<ILogger<LineToWordsProcessor>>();
      var bufferStorage = Substitute.For<IBufferStorage>();
      var lineConsumerService = Substitute.For<ILineConsumerService>();

      // Act / Assert
      Assert.Throws<ArgumentNullException>(() => new LineToWordsProcessor(logger, bufferStorage, null!, lineConsumerService));
    }

    [Fact]
    public void Constructor_WhenLineConsumerServiceIsNull_ThenThrowArgumentNullException()
    {
      // Arrange
      var logger = Substitute.For<ILogger<LineToWordsProcessor>>();
      var bufferStorage = Substitute.For<IBufferStorage>();
      var lineFileProducerService = Substitute.For<ILineFileProducerService>();

      // Act / Assert
      Assert.Throws<ArgumentNullException>(() => new LineToWordsProcessor(logger, bufferStorage, lineFileProducerService, null!));
    }

    [Fact]
    public async Task Execute_WhenLineConsumerServiceIsNull_ThenThrowArgumentNullException()
    {
      // Arrange
      var processor = new LineToWordsProcessor(Substitute.For<ILogger<LineToWordsProcessor>>(), Substitute.For<IBufferStorage>(), Substitute.For<ILineFileProducerService>(), Substitute.For<ILineConsumerService>());

      // Act / Assert
      await Assert.ThrowsAsync<ArgumentNullException>(async () => await processor.ExecuteAsync(1, 1, null!, CancellationToken.None));
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(0, 0)]
    public async Task Execute_WhenNoProducersOrNoConsumers_ThenStatusIsWrongArgumentCombination(ushort producersCount, ushort consumersCount)
    {
      // Arrange
      var processor = new LineToWordsProcessor(Substitute.For<ILogger<LineToWordsProcessor>>(), Substitute.For<IBufferStorage>(), Substitute.For<ILineFileProducerService>(), Substitute.For<ILineConsumerService>());

      // Act 
      var status = await processor.ExecuteAsync(producersCount, consumersCount, new List<string> { "file.txt" }, CancellationToken.None);

      // Assert
      Assert.Equal(ExecutionStatus.WrongArgumentCombination, status);
    }

    [Fact]
    public async Task Execute_WhenProducersAndConsumersButNoFiles_ThenStatusIsWrongArgumentCombination()
    {
      // Arrange
      var processor = new LineToWordsProcessor(Substitute.For<ILogger<LineToWordsProcessor>>(), Substitute.For<IBufferStorage>(), Substitute.For<ILineFileProducerService>(), Substitute.For<ILineConsumerService>());
      var filesToProcess = new List<string> { };

      // Act 
      var status = await processor.ExecuteAsync(1, 1, filesToProcess, CancellationToken.None);

      // Assert
      Assert.Equal(ExecutionStatus.WrongArgumentCombination, status);
    }


    [Fact]
    public async Task Execute_WhenArgumentProducersCountSet_ThenCountNumbersOfProducersCreated()
    {
      // Arrange
      var producerServiceMock = Substitute.For<ILineFileProducerService>();
      var processor = new LineToWordsProcessor(Substitute.For<ILogger<LineToWordsProcessor>>(), Substitute.For<IBufferStorage>(), producerServiceMock, Substitute.For<ILineConsumerService>());

      ushort expectedProducerCount = 2;

      // Act
      await processor.ExecuteAsync(expectedProducerCount, 1, new List<string> { "file1.txt", "file2.txt" }, CancellationToken.None);

      // Assert
      producerServiceMock.Creator().Received(expectedProducerCount);
    }

    [Fact]
    public async Task Execute_WhenArgumentProducersCountSetMoreThenFiles_ThenNumbersOfProducersAreNumberOfFiles()
    {
      // Arrange
      var producerServiceMock = Substitute.For<ILineFileProducerService>();
      var processor = new LineToWordsProcessor(Substitute.For<ILogger<LineToWordsProcessor>>(), Substitute.For<IBufferStorage>(), producerServiceMock, Substitute.For<ILineConsumerService>());
      var files = new List<string> { "file1.txt", "file2.txt" };
      ushort producersCount = 9;
      Assert.True(files.Count < producersCount, "There cannot be more then one producer pr. file.");

      // Act
      await processor.ExecuteAsync(producersCount, 1, files, CancellationToken.None);

      // Assert
      producerServiceMock.Creator().Received(files.Count);
    }

    [Fact]
    public async Task Execute_WhenArgumentConsumersCountSet_ThenCountNumbersOfConsumersCreated()
    {
      // Arrange
      var consumerServiceMock = Substitute.For<ILineConsumerService>();
      var processor = new LineToWordsProcessor(Substitute.For<ILogger<LineToWordsProcessor>>(), Substitute.For<IBufferStorage>(), Substitute.For<ILineFileProducerService>(), consumerServiceMock);

      ushort expectedConsumersCount = 2;

      // Act
      await processor.ExecuteAsync(1, expectedConsumersCount, new List<string> { "file.txt" }, CancellationToken.None);

      // Assert
      consumerServiceMock.Creator().Received(expectedConsumersCount);
    }

    [Fact]
    public async Task Execute_WhenProducersAndConsumersAreCompleted_ThenWriterAndReaderAreCompleted()
    {
      // Arrange
      var internalStorageMock = Substitute.For<IBufferStorage>();

      var processor = new LineToWordsProcessor(
          Substitute.For<ILogger<LineToWordsProcessor>>(),
          internalStorageMock,
          Substitute.For<ILineFileProducerService>(),
          Substitute.For<ILineConsumerService>());

      // Act
      await processor.ExecuteAsync(2, 2, new List<string> { "file1.txt", "file2.txt" }, CancellationToken.None);

      // Assert
      internalStorageMock.Received(1).Writer.Complete();
      await internalStorageMock.Received(1).Reader.Completion;
    }

    [Fact]
    public async Task Execute_WhenProducersAndConsumersAreCompleted_ThenExecutionIsCompleted()
    {
      // Arrange
      var internalStorageMock = Substitute.For<IBufferStorage>();

      var processor = new LineToWordsProcessor(
          Substitute.For<ILogger<LineToWordsProcessor>>(),
          internalStorageMock,
          Substitute.For<ILineFileProducerService>(),
          Substitute.For<ILineConsumerService>());

      // Act
      var status = await processor.ExecuteAsync(2, 2, new List<string> { "file1.txt", "file2.txt" }, CancellationToken.None);
      internalStorageMock.Received(1).Writer.Complete();       // Check execution ended in complete
      await internalStorageMock.Received(1).Reader.Completion; // Check execution ended in complete

      // Assert
      Assert.Equal(ExecutionStatus.ExecutionCompleted, status);
    }

    [Fact]
    public async Task Execute_WhenOneProducerAndMultipleFiles_ThenFilesAreDistributeToTheProducer()
    {
      // Arrange
      var producerServiceMock = Substitute.For<ILineFileProducerService>();
      var producerMock = Substitute.For<ILineFileProducer>();
      producerServiceMock.Creator().Returns(producerMock);

      var processor = new LineToWordsProcessor(Substitute.For<ILogger<LineToWordsProcessor>>(), Substitute.For<IBufferStorage>(), producerServiceMock, Substitute.For<ILineConsumerService>());
      var twoFiles = new List<string> { "file1.txt", "file2.txt" };

      // Act
      await processor.ExecuteAsync(1, 1, twoFiles, CancellationToken.None);

      // Assert
      var producerReceivdCalls = producerMock.ReceivedCalls();
      Assert.Single(producerReceivdCalls);
      var producerFilePaths = GetFilePathArgument(producerReceivdCalls.First());

      Assert.Equal(2, producerFilePaths.Count());
      Assert.Equal("file1.txt", producerFilePaths.ElementAt(0));
      Assert.Equal("file2.txt", producerFilePaths.ElementAt(1));
    }

    [Fact]
    public async Task Execute_WhenMultipleProducersAndMultipleFiles_ThenFilesAreDistributeToTheProducers()
    {
      // Arrange
      var producerServiceMock = Substitute.For<ILineFileProducerService>();
      var producer1Mock = Substitute.For<ILineFileProducer>();
      var producer2Mock = Substitute.For<ILineFileProducer>();
      producerServiceMock.Creator().Returns(producer1Mock, producer2Mock);

      var processor = new LineToWordsProcessor(Substitute.For<ILogger<LineToWordsProcessor>>(), Substitute.For<IBufferStorage>(), producerServiceMock, Substitute.For<ILineConsumerService>());
      var twoFiles = new List<string> { "file1.txt", "file2.txt" };
      ushort producers = (ushort)twoFiles.Count;

      // Act
      await processor.ExecuteAsync(producers, 1, twoFiles, CancellationToken.None);

      // Assert
      var producer1ReceivdCalls = producer1Mock.ReceivedCalls();
      Assert.Single(producer1ReceivdCalls);
      var producer1Argument = GetFilePathArgument(producer1ReceivdCalls.First());
      Assert.Single(producer1Argument);
      Assert.Equal("file1.txt", producer1Argument.First());

      var producer2ReceivdCalls = producer2Mock.ReceivedCalls().ToList();
      Assert.Single(producer2ReceivdCalls);
      var producer2Argument = GetFilePathArgument(producer2ReceivdCalls.First());
      Assert.Single(producer2Argument);
      Assert.Equal("file2.txt", producer2Argument.First());
    }

    private static IEnumerable<string> GetFilePathArgument(NSubstitute.Core.ICall producerReceivdCall)
    {
      var filesPath = new List<string>();

      if (producerReceivdCall.GetArguments()[0] is IEnumerable<string> firstArgument)
      {
        filesPath.AddRange(firstArgument);
      }

      return filesPath;
    }
  }
}
