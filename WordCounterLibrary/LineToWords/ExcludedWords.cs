﻿using Microsoft.Extensions.Logging;
using WordCounterLibrary.Helpers;

namespace WordCounterLibrary.LineToWords
{
  internal class ExcludedWords : IExcludedWords
  {
    private readonly string _excludedWordsFileName = "exlude.txt";

    private readonly ILogger<ExcludedWords> _logger;
    private readonly IFileReader _fileReader;
    private readonly IIOHelper _iOHelper;
    private readonly HashSet<string> _excludedWords = new();

    public ExcludedWords(ILogger<ExcludedWords> logger, IFileReader fileReader, IIOHelper iOHelper)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _fileReader = fileReader ?? throw new ArgumentNullException(nameof(fileReader));
      _iOHelper = iOHelper ?? throw new ArgumentNullException(nameof(iOHelper));
    }

    public async Task ReadExcludedWords(string directoryPath)
    {
      if (directoryPath is null) { throw new ArgumentNullException(nameof(directoryPath)); }
      var _filePath = Path.Combine(directoryPath, _excludedWordsFileName);
      if (!_iOHelper.Exists(_filePath))
      {
        _logger.LogInformation("No exlude file at {path}", _filePath);
        return;
      }

      var excludeLines = await _fileReader.ReadFileContent(_filePath);
      foreach (var excludeLine in excludeLines)
      {
        var excludedWord = excludeLine.Trim();
        if (!string.IsNullOrEmpty(excludedWord))
        {
          _excludedWords.Add(excludedWord);
        }
      }

      _logger.LogInformation("Number of excluded word(s) found is {excludedWordCount}", excludeLines.Count);
    }

    public IEnumerable<string> GetExcludedWords()
    {
      return _excludedWords.ToArray();
    }

    public bool IsExcludedWord(string word)
    {
      return _excludedWords.Contains(word);
    }
  }
}