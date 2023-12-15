namespace WordCounterLibrary.LineToWords
{
  public interface IWordStorage
  {
    void AddOrUpdate(string word, int wordCount);
    KeyValuePair<string, int>[] Snapshot();
    int WordCount { get; }
  }
}
