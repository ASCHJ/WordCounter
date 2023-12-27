
namespace WordCounterLibrary.LineToWords
{
  public interface IWordsProcessor
  {
    Task<ExecutionStatus> ExecuteAsync(ushort producersCount, ushort consumersCount, IEnumerable<string> filePaths, CancellationToken cancellationToken);
  }
}
