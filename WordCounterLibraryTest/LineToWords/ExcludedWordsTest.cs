using Microsoft.Extensions.Logging;
using NSubstitute;
using WordCounterLibrary.Helpers;
using WordCounterLibrary.LineToWords;
using Xunit;
using Xunit.Abstractions;

namespace WordCounterLibraryTest.LineToWords
{
  public class ExcludedWordsTest
  {
    private ILogger<ExcludedWords> Logger { get; }

    public ExcludedWordsTest(ITestOutputHelper outputHelper)
    {
      var loggerFactory = outputHelper.BuildLoggerFactory(LogLevel.Debug);
      Logger = loggerFactory.CreateLogger<ExcludedWords>();
    }

    [Fact]
    public async Task ReadExcludedWords_WhenFileExists_ThenExcludedWordsIsPopulatedFromFileContent()
    {
      // Arrange
      var expectedExcludeWord = "excludedWords";
      var fileExists = true;

      var fileReader = Substitute.For<IFileReader>();
      var ioHelper = Substitute.For<IIOHelper>();
      ioHelper.Exists(Arg.Any<string>()).Returns(fileExists);

      var excludedWords = new ExcludedWords(Logger, fileReader, ioHelper);

      fileReader.ReadFileContent(Arg.Any<string>()).Returns(await Task.FromResult(new List<string> { expectedExcludeWord }));

      // Act
      await excludedWords.ReadExcludedWords("directoryPath");

      // Assert
      Assert.Single(excludedWords.GetExcludedWords().ToList());
      Assert.Equal(expectedExcludeWord, excludedWords.GetExcludedWords().First());
    }

    [Fact]
    public async Task ReadExcludedWords_WhenFileDoesNotExist_ThenThereAreNoExcludedWords()
    {
      // Arrange
      var fileExists = false;

      var fileReader = Substitute.For<IFileReader>();
      var ioHelper = Substitute.For<IIOHelper>();
      ioHelper.Exists(Arg.Any<string>()).Returns(fileExists);

      var excludedWords = new ExcludedWords(Logger, fileReader, ioHelper);

      // Act
      await excludedWords.ReadExcludedWords("");

      // Assert
      await fileReader.DidNotReceive().ReadFileContent(Arg.Any<string>());
      Assert.Empty(excludedWords.GetExcludedWords());
    }

    [Fact]
    public async Task IsExcludedWord_WhenExcludedWordExists_ThenReturnTrue()
    {
      // Arrange
      var excludedWord = "excludedWord";
      var fileReader = Substitute.For<IFileReader>();
      var ioHelper = Substitute.For<IIOHelper>();
      ioHelper.Exists(Arg.Any<string>()).Returns(true);
      var excludedWords = new ExcludedWords(Logger, fileReader, ioHelper);

      fileReader.ReadFileContent(Arg.Any<string>()).Returns(await Task.FromResult(new List<string> { excludedWord }));

      // Act
      await excludedWords.ReadExcludedWords("");
      var result = excludedWords.IsExcludedWord("excludedWord");

      // Assert
      Assert.True(result);
    }

    [Fact]
    public async Task IsExcludedWord_WhenExcludedWordDoesNotExists_ThenReturnsFalse()
    {
      // Arrange
      var fileReader = Substitute.For<IFileReader>();
      var ioHelper = Substitute.For<IIOHelper>();
      ioHelper.Exists(Arg.Any<string>()).Returns(true);
      var excludedWords = new ExcludedWords(Logger, fileReader, ioHelper);

      fileReader.ReadFileContent(Arg.Any<string>()).Returns(await Task.FromResult(new List<string>()));

      // Act
      await excludedWords.ReadExcludedWords("");
      var result = excludedWords.IsExcludedWord("somenoneexistingword");

      // Assert
      Assert.False(result);
    }
  }
}
