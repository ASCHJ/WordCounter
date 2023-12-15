namespace WordCounterLibrary.LineToWords
{
  internal interface IBufferStorage
  {
    IBufferReader Reader { get; }
    IBufferWriter Writer { get; }
  }
}
