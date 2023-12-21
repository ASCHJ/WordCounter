namespace WordCounterLibrary.Common
{
  public interface IFileManagement
  {
    bool Exists(string filePath);
    string CurrentDirectory { get; }
  }
}
