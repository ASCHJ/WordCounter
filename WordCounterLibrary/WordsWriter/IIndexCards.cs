
namespace WordCounterLibrary.WordsWriter
{
  internal interface IIndexCards
  {
    void Add(string character, int index);
    void CreateIndexKey(char character);
    IEnumerable<KeyValuePair<char, HashSet<int>>> GetIndexCards();
    void AddExcludedIndex(int index);
    IEnumerable<int> GetExcludedIndexCards();
  }
}
