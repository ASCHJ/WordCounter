using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace WordCounterLibrary.LineToWords
{
  internal interface IFileReader
  {
    Task WriteFileContentToBufferAsync(string filePath);
    Task<IReadOnlyList<string>> ReadFileContent(string filePath);
  }
}
