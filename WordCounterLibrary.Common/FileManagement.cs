using System.Reflection;

namespace WordCounterLibrary.Common
{
  public class FileManagement : IFileManagement
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
  }
}
