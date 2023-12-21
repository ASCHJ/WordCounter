using System.Text.RegularExpressions;

namespace WordCounterTest.Helpers
{
  internal class FileManagement
  {
    public static void FileContentCopy(string sourceFilePath, string destinationFilePath)
    {
      try
      {
        if (File.Exists(sourceFilePath) && File.Exists(destinationFilePath))
        {
          using StreamReader reader = new(sourceFilePath);
          using StreamWriter writer = new(destinationFilePath, false);
          char[] buffer = new char[4096];
          int bytesRead;

          while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
          {
            writer.Write(buffer, 0, bytesRead);
          }
        }
        else
        {
          throw new FileNotFoundException($"Following files may does not exist. SourceFilePath '{sourceFilePath}' or DestinationFilePath '{destinationFilePath}'");
        }
      }
      catch (Exception)
      {
        throw;
      }
    }

    public static int SearchForCaseInsensitiveWord(string filePath, string outputFilepattern)
    {
      var wordSum = 0;
      try
      {
        using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read);
        using StreamReader reader = new(fileStream);
        {
          while (!reader.EndOfStream)
          {
            string? line = reader.ReadLine();
            if (line != null)
            {
              wordSum += OccurrencesOfWordCaseIgnored(line, outputFilepattern);
            }
          }
        }
      }
      catch (Exception)
      {
        throw;
      }

      return wordSum;
    }

    public static int GetWordCountFromMatchingPattern(string expectedOutputFile, string outputFilepattern)
    {
      return GetWordCountFromMatchingPattern(new List<string> { expectedOutputFile }, outputFilepattern);
    }

    public static int GetWordCountFromMatchingPattern(IEnumerable<string> expectedOutputFiles, string outputFilepattern)
    {
      var wordSum = 0;

      foreach (var filePath in expectedOutputFiles)
      {
        try
        {
          using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read);
          using StreamReader reader = new(fileStream);
          while (!reader.EndOfStream)
          {
            string? line = reader.ReadLine();
            if (line != null)
            {
              wordSum += ParseForNumber(line, outputFilepattern);
            }
          }
        }
        catch (Exception)
        {
          throw;
        }
      }

      return wordSum;
    }

    private static int ParseForNumber(string input, string pattern)
    {
      Match match = Regex.Match(input, pattern);
      if (match.Success && int.TryParse(match.Groups[1].Value, out int parsedNumber))
      {
        return parsedNumber;
      }
      else
      {
        return 0;
      }
    }

    private static int OccurrencesOfWordCaseIgnored(string input, string word)
    {
      var pattern = $@"\b{word}\b"; //  \b allows a "whole words only" search
      MatchCollection matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase);
      return matches.Count;
    }
  }
}
