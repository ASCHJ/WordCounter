namespace WordCounterLibrary.Format
{
  internal interface IWordAndCountFormat
  {
    void AppendLine(string word, int count);
    string ToString();
  }
}
