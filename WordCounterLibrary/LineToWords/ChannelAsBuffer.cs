using System.Threading.Channels;

namespace WordCounterLibrary.LineToWords
{
  internal class ChannelAsBuffer : IBufferStorage
  {
    private readonly Channel<string> channel;
    public int MaxMessagesToBuffer => 500;

    public IBufferReader Reader { get; }
    public IBufferWriter Writer { get; }

    public ChannelAsBuffer()
    {
      channel = Channel.CreateBounded<string>(MaxMessagesToBuffer);

      Reader = new ChannelAsBufferReader(channel.Reader);
      Writer = new ChannelAsBufferWriter(channel.Writer);
    }
  }
}
