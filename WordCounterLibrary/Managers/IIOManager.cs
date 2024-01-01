namespace WordCounterLibrary.Managers
{
  public interface IIOManager
  {
    bool Exists(string filePath);
    string CurrentDirectory { get; }
    string[] GetFilesInDirectory(string directoryPath, string searchPattern);
  }
}
