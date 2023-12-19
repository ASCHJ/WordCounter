namespace WordCounterLibrary.LineToWords
{
  internal interface IBufferStorage
  {
    int MaxMessagesToBuffer { get; }
    IBufferReader Reader { get; }
    IBufferWriter Writer { get; }
  }
}
