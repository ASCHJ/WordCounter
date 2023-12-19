namespace WordCounterLibrary.WordsWriter
{
  internal interface IArchiver
  {
    void Archive(IIndexCards indexCards);
    void ArchiveExcluded(IIndexCards indexCards);
  }
}
