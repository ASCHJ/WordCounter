
namespace WordCounterLibrary.LineToWords
{
  internal interface ILineConsumer
  {
    Task ConsumeAsync(CancellationToken cancellationToken = default);
  }
}
