using Microsoft.Extensions.Logging;
using shortid;
using WordCounterLibrary.Format;
using WordCounterLibrary.Helpers;
using WordCounterLibrary.LineToWords;
using WordCounterLibrary.Repository;

namespace WordCounterLibrary.WordsWriter
{
  internal class FileArchiver : IArchiver
  {
    private readonly string _excludedFilename = "excludedReport.txt";

    private readonly string _filePostfix = "FILE_";
    private readonly string _fileExtension = ".txt";

    private readonly ILogger<FileArchiver> _logger;
    private readonly IWordRepository _wordRepository;
    private readonly IExcludedWords _excludedWords;
    private readonly IIOHelper _iOHelper;

    public FileArchiver(ILogger<FileArchiver> logger, IWordRepository wordRepository, IExcludedWords excludedWords, IIOHelper iOHelper)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _wordRepository = wordRepository ?? throw new ArgumentNullException(nameof(wordRepository));
      _excludedWords = excludedWords ?? throw new ArgumentNullException(nameof(excludedWords));
      _iOHelper = iOHelper ?? throw new ArgumentNullException(nameof(iOHelper));
    }

    public void Archive(IIndexCards indexCards)
    {
      Parallel.ForEach(indexCards.GetIndexCards(), entry =>
      {
        var fileName = $"{_filePostfix}{entry.Key}{_fileExtension}";
        var outputFile = Path.Combine(_iOHelper.CurrentDirectory, fileName);
        var id = ShortId.Generate();

        _logger.LogInformation("Archiver '{id}' writting file '{filePathAndName}'", id, outputFile);

        string content = string.Empty;
        if (entry.Value.Any())
        {
          var outputFormat = new WordAndCountFormat();
          foreach (var wordIndex in entry.Value)
          {
            var (word, count) = _wordRepository.ElementAtOrDefault(wordIndex);
            outputFormat.AppendLine(word, count);
          }

          content = outputFormat.ToString();
        }

        File.WriteAllText(outputFile, content); //TODo FIX
        _logger.LogInformation("Archiver '{id}' done writting file '{filePathAndName}'", id, outputFile);
      });
    }

    public void ArchiveExcluded(IIndexCards indexCards)
    {
      var outputFile = Path.Combine(_iOHelper.CurrentDirectory, _excludedFilename);
      var id = ShortId.Generate();
      _logger.LogInformation("Creating excluded report. Writting file '{filePathAndName}'", outputFile);

      var outputFormat = new ReportFormat();
      var excludedWords = _excludedWords.GetExcludedWords().ToList();
      foreach (var wordIndex in indexCards.GetExcludedIndexCards())
      {
        var (word, count) = _wordRepository.ElementAtOrDefault(wordIndex);
        outputFormat.AppendLine(word, count);
        excludedWords.Remove(word);
      }

      foreach (var excludedWord in excludedWords)
      {
        outputFormat.AppendLine(excludedWord, 0);
      }

      var content = outputFormat.ToString();
      File.WriteAllText(outputFile, content); //TODo FIX
      _logger.LogInformation("Archiver '{id}' done writting excluded report to file '{filePathAndName}'", id, outputFile);
    }
  }
}
