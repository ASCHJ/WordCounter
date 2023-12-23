namespace WordCounterLibrary.Format
{
  internal interface IFormatter
  {
    void AppendLine(string word, int count);
    string GetContent();
  }
}
