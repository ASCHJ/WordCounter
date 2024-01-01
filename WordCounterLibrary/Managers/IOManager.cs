using System.Reflection;

namespace WordCounterLibrary.Managers
{
  internal class IOManager : IIOManager
  {
    public string CurrentDirectory => GetCurrentDirectory();

    public static string GetCurrentDirectory()
    {
      var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().Location);
      var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
      return Path.GetDirectoryName(codeBasePath) ?? throw new DirectoryNotFoundException("Could not find directory name");
    }

    public bool Exists(string filePath)
    {
      return File.Exists(filePath);
    }

    public string[] GetFilesInDirectory(string directoryPath, string searchPattern)
    {
      if (Directory.Exists(directoryPath))
      {
        return Directory.GetFiles(directoryPath, searchPattern);
      }
      else
      {
        return Array.Empty<string>();
      }
    }
  }
}
