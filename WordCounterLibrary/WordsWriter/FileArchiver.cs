using Microsoft.Extensions.Logging;
using shortid;
using WordCounterLibrary.Format;
using WordCounterLibrary.IO;
using WordCounterLibrary.Managers;
using WordCounterLibrary.Repository;

namespace WordCounterLibrary.WordsWriter
{
  internal class FileArchiver : IArchiver
  {
    private readonly string _excludedFilename = "ExcludeReport.txt";

    private readonly string _filePostfix = "FILE_";
    private readonly string _fileExtension = ".txt";

    private readonly ILogger<FileArchiver> _logger;
    private readonly IWordRepository _wordRepository;
    private readonly IFormatFactory _formatFactory;
    private readonly IIOManager _iOManager;
    private readonly IFileWriter _fileWriter;

    public FileArchiver(ILogger<FileArchiver> logger, IWordRepository wordRepository, IFormatFactory formatFactory, IIOManager iOManager, IFileWriter fileWriter)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _wordRepository = wordRepository ?? throw new ArgumentNullException(nameof(wordRepository));
      _formatFactory = formatFactory ?? throw new ArgumentNullException(nameof(formatFactory));
      _iOManager = iOManager ?? throw new ArgumentNullException(nameof(iOManager));
      _fileWriter = fileWriter ?? throw new ArgumentNullException(nameof(fileWriter));
    }

    public void Archive(IIndexCards indexCards)
    {
      if (indexCards is null) { throw new ArgumentNullException(nameof(indexCards));  }

      Parallel.ForEach(indexCards.GetIndexCards(), entry =>
      {
        var fileName = $"{_filePostfix}{entry.Key}{_fileExtension}";
        var outputFile = Path.Combine(_iOManager.CurrentDirectory, fileName);
        var id = ShortId.Generate();

        _logger.LogInformation("Archiver '{id}' writting file '{filePathAndName}'.", id, outputFile);

        string content = string.Empty;
        var outputFormat = _formatFactory.CreateFormat<WordAndCountFormat>();
        foreach (var wordIndex in entry.Value)
        {
          var (word, count) = _wordRepository.ElementAtOrDefault(wordIndex);
          outputFormat.AppendLine(word, count);
        }

        content = outputFormat.GetContent();
        _fileWriter.Write(outputFile, content);
        
        _logger.LogInformation("Archiver '{id}' done writting file '{filePathAndName}'.", id, outputFile);
      });
    }

    public void ArchiveExcluded(IIndexCards indexCards)
    {
      if (indexCards is null) { throw new ArgumentNullException(nameof(indexCards)); }

      var outputFile = Path.Combine(_iOManager.CurrentDirectory, _excludedFilename);
      var id = ShortId.Generate();
      _logger.LogInformation("Creating excluded report.");

      var outputFormat = _formatFactory.CreateFormat<ReportFormat>();
      foreach (var wordIndex in indexCards.GetExcludedIndexCards())
      {
        var (word, count) = _wordRepository.ElementAtOrDefault(wordIndex);
        outputFormat.AppendLine(word, count);
      }

      var content = outputFormat.GetContent();
      if(!string.IsNullOrEmpty(content))
      {
        _fileWriter.Write(outputFile, content);
        _logger.LogInformation("Archiver '{id}' done writting excluded report to file '{filePathAndName}'.", id, outputFile);
      }
      else
      {
        _logger.LogInformation("Archiver '{id}' none excluded report to write, because report content is empty.", id);      }
      
    }
  }
}
