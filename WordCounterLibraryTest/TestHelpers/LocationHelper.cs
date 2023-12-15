using System.Reflection;

namespace WordCounterLibraryTest.TestHelpers
{
  internal class LocationHelper
  {
    public static string CurrentDirectory(string relativePath)
    {
      var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().Location);
      var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
      var dirPath = Path.GetDirectoryName(codeBasePath) ?? throw new ArgumentNullException("Could not find directory name");
      return Path.Combine(dirPath, relativePath);
    }
  }
}
