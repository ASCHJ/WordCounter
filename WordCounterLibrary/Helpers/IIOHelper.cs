namespace WordCounterLibrary.Helpers
{
  internal interface IIOHelper
  {
    bool Exists(string filePath);
    string CurrentDirectory { get; }
  }
}
