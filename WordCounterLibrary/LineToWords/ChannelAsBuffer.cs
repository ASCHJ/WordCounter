using System.Threading.Channels;

namespace WordCounterLibrary.LineToWords
{
  internal class ChannelAsBuffer : IBufferStorage
  {
    private int MaxMessagesToBuffer { get; }
    private readonly Channel<string> channel;

    public IBufferReader Reader { get; }
    public IBufferWriter Writer { get; }

    public ChannelAsBuffer(int maxMessagesToBuffer)
    {
      MaxMessagesToBuffer = maxMessagesToBuffer;
      channel = Channel.CreateBounded<string>(MaxMessagesToBuffer);

      Reader = new ChannelAsBufferReader(channel.Reader);
      Writer = new ChannelAsBufferWriter(channel.Writer);
    }
  }
}
