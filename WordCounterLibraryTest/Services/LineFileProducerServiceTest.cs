using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using WordCounterLibrary.LineToWords;
using WordCounterLibrary.Services;
using Xunit;

namespace WordCounterLibraryTest.Services
{
  public class LineFileProducerServiceTest
  {
    private readonly ILogger<LineFileProducer> _logger = new NullLogger<LineFileProducer>();

    [Fact]
    public void Creator_WhenCreatorIsCalled_ThenReturnLineFileProducer()
    {
      // Arrange
      var bufferStorageMock = Substitute.For<IBufferStorage>();
      var fileReaderService = Substitute.For<IFileReaderService>();

      var lineConsumerService = new LineFileProducerService(
          _logger,
          bufferStorageMock,
          fileReaderService
      );

      // Act
      var lineFileProducer = lineConsumerService.Creator();

      // Assert
      Assert.NotNull(lineFileProducer);
      Assert.IsType<LineFileProducer>(lineFileProducer);
    }

    [Fact]
    public void Constructor_WhenLoggerIsNull_ThenThrowsArgumentNullException()
    {
      // Arrange
      var bufferStorageMock = Substitute.For<IBufferStorage>();
      var fileReaderService = Substitute.For<IFileReaderService>();

      // Act & Assert
      Assert.Throws<ArgumentNullException>(() => new LineFileProducerService(null!, bufferStorageMock, fileReaderService));
    }

    [Fact]
    public void Constructor_WhenBufferStorageIsNull_ThenThrowsArgumentNullException()
    {
      // Arrange
      var fileReaderServiceMock = Substitute.For<IFileReaderService>();

      // Act & Assert
      Assert.Throws<ArgumentNullException>(() => new LineFileProducerService(_logger, null!, fileReaderServiceMock));
    }

    [Fact]
    public void Constructor_WhenFileReaderIsNull_ThenThrowsArgumentNullException()
    {
      // Arrange
      var bufferStorageMock = Substitute.For<IBufferStorage>();

      // Act & Assert
      Assert.Throws<ArgumentNullException>(() => new LineFileProducerService(_logger, bufferStorageMock, null!));
    }
  }
}
