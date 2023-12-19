using System.Reflection;

namespace WordCounterLibraryTest.TestHelpers
{
  internal class LocationHelper
  {
    public static string GetDirectory(string relativePath)
    {
      var dirPath = CurrentDirectory();
      return Path.Combine(dirPath, relativePath);
    }

    public static string CurrentDirectory()
    {
      var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().Location);
      var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
      var dirPath = Path.GetDirectoryName(codeBasePath) ?? throw new ArgumentNullException("Could not find directory name");
      return dirPath;
    }
  }
}
