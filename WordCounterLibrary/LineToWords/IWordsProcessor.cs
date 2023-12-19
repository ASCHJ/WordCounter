
namespace WordCounterLibrary.LineToWords
{
  public interface IWordsProcessor
  {
    Task Execute(int producersCount, int consumersCount, IEnumerable<string> filePaths, CancellationToken cancellationToken);
  }
}
