
namespace WordCounterLibrary.Managers
{
  internal interface IExcludeManager
  {
    Task FillExcludeRepositoryWithExcludeWordsFromFile(string directoryPath);
  }
}