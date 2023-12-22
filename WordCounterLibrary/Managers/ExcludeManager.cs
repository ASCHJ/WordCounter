using Microsoft.Extensions.Logging;
using WordCounterLibrary.Repository;
using WordCounterLibrary.Services;

namespace WordCounterLibrary.Managers
{
  internal class ExcludeManager : IExcludeManager
  {
    private readonly ILogger<ExcludedWordsRepository> _logger;
    private readonly IExcludedWordsRepository _excludedWordsRepository;
    private readonly IIOManager _iOManager;
    private readonly IFileReaderService _fileReaderService;

    private readonly string _excludedWordsFileName = "exlude.txt";

    public ExcludeManager(ILogger<ExcludedWordsRepository> logger, IExcludedWordsRepository excludedWordsRepository, IIOManager iOManager, IFileReaderService fileReaderService)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _excludedWordsRepository = excludedWordsRepository ?? throw new ArgumentNullException(nameof(excludedWordsRepository));
      _iOManager = iOManager ?? throw new ArgumentNullException(nameof(iOManager));
      _fileReaderService = fileReaderService ?? throw new ArgumentNullException(nameof(fileReaderService));
    }

    public async Task FillExcludeRepositoryWithExcludeWordsFromFile(string directoryPath)
    {
      if (directoryPath is null) { throw new ArgumentNullException(nameof(directoryPath)); }

      var _filePath = Path.Combine(directoryPath, _excludedWordsFileName);
      if (!_iOManager.Exists(_filePath))
      {
        _logger.LogInformation("No exlude file at {path}", _filePath);
        return;
      }

      using (var reader = _fileReaderService.GetReader(_filePath))
      {
        while (!reader.EndOfStream)
        {
          var excludeLine = await reader.ReadLineAsync();
          if (!string.IsNullOrEmpty(excludeLine))
          {
            var excludedWord = excludeLine.Trim();
            _excludedWordsRepository.Add(excludedWord);
          }
        }
      }

      _logger.LogInformation("Number of excluded word(s) found is {excludedWordCount}", _excludedWordsRepository.Count);
    }
  }
}
