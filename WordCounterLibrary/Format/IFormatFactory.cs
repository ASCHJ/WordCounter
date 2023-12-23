using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace WordCounterLibrary.Format
{
  internal interface IFormatFactory
  {
    IFormatter CreateFormat<T>() where T : IFormatter, new();
  }
}
