namespace WordCounterLibrary.Format
{
  internal class FormatFactory : IFormatFactory
  {
    public IFormatter CreateFormat<T>() where T : IFormatter, new()
    {
      return typeof(T).Name switch
      {
        nameof(WordAndCountFormat) => new WordAndCountFormat(),
        nameof(ReportFormat) => new ReportFormat(),
        _ => new ReportFormat(),
      };
    }
  }
}
