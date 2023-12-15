namespace WordCounterLibrary.Helpers
{
  public class RetryException : Exception
  {
    public RetryException() { }

    public RetryException(string message) : base(message) { }

    public RetryException(string message, Exception inner) : base(message, inner) { }
  }
}
