using WordCounterLibrary.Repository;

namespace WordCounterLibrary.WordsWriter
{
  internal class FileReporter : IReporter
  {
    private readonly IWordRepository _wordRepository;
    private readonly IArchiver _archiver;
    private readonly IExcludedWordsRepository _excludedWords;
    private readonly IIndexCards _indexCards;
    private readonly IAlphabet alphabet;

    public FileReporter(IWordRepository wordRepository, IArchiver archiver, IExcludedWordsRepository excludedWords, IIndexCards indexCards, IAlphabet alphabet)
    {
      _wordRepository = wordRepository ?? throw new ArgumentNullException(nameof(wordRepository));
      _archiver = archiver ?? throw new ArgumentNullException(nameof(archiver));
      _excludedWords = excludedWords ?? throw new ArgumentNullException(nameof(excludedWords));
      _indexCards = indexCards ?? throw new ArgumentNullException(nameof(indexCards));
      this.alphabet = alphabet ?? throw new ArgumentNullException(nameof(alphabet));
    }

    public void WriteReports()
    {
      CreateIndexCardsMatchingTheAlphabet(alphabet.Get());
      FillIndexCards(_wordRepository);
      ArchiveToFile();
    }

    private void CreateIndexCardsMatchingTheAlphabet(IEnumerable<char> alphabet)
    {
      foreach (var character in alphabet)
      {
        _indexCards.CreateIndexKey(character);
      }
    }

    private void FillIndexCards(IWordRepository words)
    {
      for (int index = 0; index < words.Count; index++)
      {
        var (key, _) = words.ElementAtOrDefault(index);
        if (_excludedWords.IsExcludedWord(key))
        {
          _indexCards.AddExcludedIndex(index);
        }
        else
        {
          _indexCards.Add(key, index);
        }
      }
    }

    private void ArchiveToFile()
    {
      _archiver.Archive(_indexCards);
      _archiver.ArchiveExcluded(_indexCards);
    }
  }
}
