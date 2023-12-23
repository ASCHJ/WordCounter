using System.Globalization;
using System.Text;

namespace WordCounterLibrary.Format
{
  internal class WordAndCountFormat : IFormatter
  {
    private readonly StringBuilder wordEntryBuilder = new();

    public void AppendLine(string word, int count)
    {
      if(!string.IsNullOrEmpty(word)) { 
      wordEntryBuilder.Append("  ").Append(HandleUpperCase(word)).Append(' ').AppendLine(count.ToString(CultureInfo.InvariantCulture));
      }
    }

    public string GetContent()
    {
      return wordEntryBuilder.ToString();
    }

    private static string HandleUpperCase(string inputStr)
    {
      return IsAllUpperCase(inputStr) ? inputStr : inputStr.ToUpper();
    }

    private static bool IsAllUpperCase(string input)
    {
      return input.All(char.IsUpper);
    }
  }
}
