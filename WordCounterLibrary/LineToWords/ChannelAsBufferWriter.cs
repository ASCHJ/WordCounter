using System.Threading.Channels;

namespace WordCounterLibrary.LineToWords
{
  internal class ChannelAsBufferWriter : IBufferWriter
  {
    private readonly ChannelWriter<string> _writer;

    public ChannelAsBufferWriter(ChannelWriter<string> writer)
    {
      _writer = writer ?? throw new ArgumentNullException(nameof(writer));
    }

    public void Complete()
    {
      _writer.Complete();
    }

    public async Task WriteAsync(string message, CancellationToken cancellationToken)
    {
      await _writer.WriteAsync(message, cancellationToken).ConfigureAwait(false);
    }
  }
}
