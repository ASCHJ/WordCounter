using System.Collections.Concurrent;
using WordCounterLibrary.LineToWords;
using Xunit;

namespace WordCounterLibraryTest.LineToWords
{
  public class ChannelAsBufferIntegrationsTest
  {
    [Fact]
    public async Task ChannelAsBuffer_WhenWritingAndReadingFromChannel_ThenReturnsLine()
    {
      // Arrange
      var expectedResult = "line";
      var buffer = new ChannelAsBuffer();
      var writer = buffer.Writer;
      var reader = buffer.Reader;

      // Act
      await writer.WriteAsync(expectedResult, CancellationToken.None);
      writer.Complete();

      var result = string.Empty;
      await foreach (var line in reader.ReadAllAsync(CancellationToken.None))
      {
        result = line;
      }

      // Assert
      Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task ChannelAsBuffer_WhenWritingMultipleWordsAndReadingFromChannel_ThenReturnsMultipleLines()
    {
      // Arrange
      var expectedResult1 = "line 1";
      var expectedResult2 = "line 2";
      var buffer = new ChannelAsBuffer();
      var writer = buffer.Writer;
      var reader = buffer.Reader;

      // Act
      await writer.WriteAsync(expectedResult1, CancellationToken.None);
      await writer.WriteAsync(expectedResult2, CancellationToken.None);
      writer.Complete();

      var results = new ConcurrentBag<string>();
      await foreach (var line in reader.ReadAllAsync(CancellationToken.None))
      {
        results.Add(line);
      }

      // Assert
      Assert.Equal(2, results.Count);
      Assert.Equal(expectedResult1, results.ElementAt(1));
      Assert.Equal(expectedResult2, results.ElementAt(0));
    }
  }
}
