namespace WordCounterLibrary.Managers
{
  internal interface IIOManager
  {
    bool Exists(string filePath);
    string CurrentDirectory { get; }
  }
}
