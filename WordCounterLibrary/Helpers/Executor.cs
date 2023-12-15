using Microsoft.Extensions.Logging;

namespace WordCounterLibrary.Helpers
{
  internal class Executor : IExecutor //TODO: temporary class?
  {
    private readonly ILogger Logger;

    public Executor(ILogger logger)
    {
      this.Logger = logger;
    }
    public void Retries(Func<bool> func, int numberOfRetries)
    {
      int attempt = 1;

      while (attempt <= numberOfRetries)
      {
        bool methodResult = func();

        if (methodResult)
        {
          return;
        }

        Logger.LogDebug("Attempt {attempt} failed. Retrying...", attempt);
        attempt++;
      }

      Logger.LogError("Error after {attempt} tries.", attempt - 1);
      throw new RetryException($"Error after {attempt - 1} tries");
    }
  }
}
