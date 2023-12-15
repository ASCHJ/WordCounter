using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("WordCounterLibraryTest")]

namespace WordCounterLibrary.Format
{
  internal class LipsumLineFormatParser : ILineFormatParser
  {
    private static readonly char[] Separators = { ' ', '.', ',', '\n', ';', '\r' };

    public string[] GetWords(string inputString)
    {
      if (inputString == null)
      {
        throw new ArgumentNullException(nameof(inputString));
      }

      return inputString.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
    }
  }
}
