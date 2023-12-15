namespace WordCounterLibrary.Helpers
{
  public interface IExecutor
  {
    void Retries(Func<bool> func, int numberOfRetries);
  }
}
