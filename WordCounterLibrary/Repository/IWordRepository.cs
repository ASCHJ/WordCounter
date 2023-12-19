namespace WordCounterLibrary.Repository
{
  public interface IWordRepository
  {
    void AddOrUpdate(string word, int wordCount);
    KeyValuePair<string, int> ElementAtOrDefault(int index);
    int Count { get; }
  }
}
