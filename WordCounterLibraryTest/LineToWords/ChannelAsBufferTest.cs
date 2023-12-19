using WordCounterLibrary.LineToWords;
using Xunit;

namespace WordCounterLibraryTest.LineToWords
{
  public class ChannelAsBufferTest
  {
    [Fact]
    public void Initialization_WhenChannelAsBufferIsInitialized_ThenSoIsWriterBuffer()
    {
      // Arrange
      var buffer = new ChannelAsBuffer();

      // Assert
      Assert.NotNull(buffer.Writer);
    }

    [Fact]
    public void Initialization_WhenChannelAsBufferIsInitialized_ThenSoIsReaderBuffer()
    {
      // Arrange
      var buffer = new ChannelAsBuffer();

      // Assert
      Assert.NotNull(buffer.Reader);
    }

    [Fact]
    public void Initialization_WhenChannelAsBufferIsInitialized_ThenMaxMessagesToBufferIs500()
    {
      // Arrange
      var expectedMaxMessages = 500;
      var buffer = new ChannelAsBuffer();

      // Assert
      Assert.Equal(expectedMaxMessages, buffer.MaxMessagesToBuffer);
    }
  }
}
