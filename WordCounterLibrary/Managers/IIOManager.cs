namespace WordCounterLibrary.Helpers
{
  internal interface IIOManager
  {
    bool Exists(string filePath);
    string CurrentDirectory { get; }
  }
}
