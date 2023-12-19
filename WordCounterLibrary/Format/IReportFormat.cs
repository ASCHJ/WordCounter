namespace WordCounterLibrary.Format
{
  internal interface IReportFormat
  {
    void AppendLine(string word, int count);
    string ToString();
  }
}
