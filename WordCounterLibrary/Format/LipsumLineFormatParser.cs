using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("WordCounterLibraryTest")]

namespace WordCounterLibrary.Format
{
  internal class LipsumLineFormatParser : ILineFormatParser
  {
    private static readonly char[] Separators = { ' ', '.', ',', ';' };

    public IEnumerable<KeyValuePair<string, int>> GetWordKeyPairs(string line)
    {
      if (line == null) { throw new ArgumentNullException(nameof(line)); }

      var words = GetWords(line);
      var wordsOccurrences = GroupWordsByOccurence(words);
      return wordsOccurrences;
    }

    private static IEnumerable<KeyValuePair<string, int>> GroupWordsByOccurence(IEnumerable<string> words)
    {
      return words.GroupBy(word => word)
                       .Select(group => new KeyValuePair<string, int>(group.Key, group.Count()));
    }

    private static IEnumerable<string> GetWords(string inputString)
    {
      return inputString.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
    }
  }
}
