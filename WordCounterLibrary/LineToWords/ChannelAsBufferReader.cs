using System.Threading.Channels;

namespace WordCounterLibrary.LineToWords
{
  internal class ChannelAsBufferReader : IBufferReader
  {
    private readonly ChannelReader<string> _reader;

    public Task Completion => _reader?.Completion ?? Task.CompletedTask;

    public ChannelAsBufferReader(ChannelReader<string> reader)
    {
      _reader = reader ?? throw new ArgumentNullException(nameof(reader));
    }

    public IAsyncEnumerable<string> ReadAllAsync(CancellationToken cancellationToken)
    {
      return _reader.ReadAllAsync(cancellationToken);
    }
  }
}
