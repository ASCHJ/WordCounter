
namespace WordCounterLibrary.LineToWords
{
  internal interface ILineFileProducer
  {
    Task ProduceAsync(IEnumerable<string> filePaths, CancellationToken cancellationToken = default);
  }
}
