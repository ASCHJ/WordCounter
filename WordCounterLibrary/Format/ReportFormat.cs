using System.Globalization;
using System.Text;

namespace WordCounterLibrary.Format
{
  internal class ReportFormat : IFormatter
  {
    private readonly StringBuilder wordEntryBuilder = new();

    public void AppendLine(string word, int count)
    {
      if (!string.IsNullOrEmpty(word))
      {
        wordEntryBuilder.Append(word).Append(' ').AppendLine(count.ToString(CultureInfo.InvariantCulture));
      }
    }

    public string GetContent()
    {
      return wordEntryBuilder.ToString();
    }
  }
}
