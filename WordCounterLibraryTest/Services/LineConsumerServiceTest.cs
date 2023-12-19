using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using WordCounterLibrary.Format;
using WordCounterLibrary.LineToWords;
using WordCounterLibrary.Repository;
using WordCounterLibrary.Services;
using Xunit;

namespace WordCounterLibraryTest.LineToWords
{
  public class LineConsumerServiceTest
  {
    private readonly ILogger<LineConsumer> _logger = new NullLogger<LineConsumer>();

    [Fact]
    public void Creator_WhenCreatorIsCalled_ThenReturnsConsumer()
    {
      // Arrange
      var bufferStorageMock = Substitute.For<IBufferStorage>();
      var lineFormatParserMock = Substitute.For<ILineFormatParser>();
      var wordStorageMock = Substitute.For<IWordRepository>();

      var lineConsumerService = new LineConsumerService(
          _logger,
          bufferStorageMock,
          lineFormatParserMock,
          wordStorageMock
      );

      // Act
      var lineConsumer = lineConsumerService.Creator();

      // Assert
      Assert.NotNull(lineConsumer);
      Assert.IsType<LineConsumer>(lineConsumer);
    }

    [Fact]
    public void Constructor_WhenLoggerIsNull_ThenThrowsArgumentNullException()
    {
      // Arrange
      var bufferStorageMock = Substitute.For<IBufferStorage>();
      var lineFormatParserMock = Substitute.For<ILineFormatParser>();
      var wordStorageMock = Substitute.For<IWordRepository>();

      // Act & Assert
      Assert.Throws<ArgumentNullException>(() => new LineConsumerService(null!, bufferStorageMock, lineFormatParserMock, wordStorageMock));
    }

    [Fact]
    public void Constructor_WhenBufferStorageIsNull_ThenThrowsArgumentNullException()
    {
      // Arrange
      var bufferStorageMock = Substitute.For<IBufferStorage>();
      var lineFormatParserMock = Substitute.For<ILineFormatParser>();
      var wordStorageMock = Substitute.For<IWordRepository>();

      // Act & Assert
      Assert.Throws<ArgumentNullException>(() => new LineConsumerService(_logger, null!, lineFormatParserMock, wordStorageMock));
    }

    [Fact]
    public void Constructor_WhenLineFormatParserIsNull_ThenThrowsArgumentNullException()
    {
      // Arrange
      var bufferStorageMock = Substitute.For<IBufferStorage>();
      var wordStorageMock = Substitute.For<IWordRepository>();

      // Act & Assert
      Assert.Throws<ArgumentNullException>(() => new LineConsumerService(_logger, bufferStorageMock, null!, wordStorageMock));
    }

    [Fact]
    public void Constructor_WhenWordStorageIsNull_ThenThrowsArgumentNullException()
    {
      // Arrange
      var bufferStorageMock = Substitute.For<IBufferStorage>();
      var lineFormatParserMock = Substitute.For<ILineFormatParser>();

      // Act & Assert
      Assert.Throws<ArgumentNullException>(() => new LineConsumerService(_logger, bufferStorageMock, lineFormatParserMock, null!));
    }
  }
}
