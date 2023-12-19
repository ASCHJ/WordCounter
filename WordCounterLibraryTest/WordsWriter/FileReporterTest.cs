using NSubstitute;
using WordCounterLibrary.LineToWords;
using WordCounterLibrary.Repository;
using WordCounterLibrary.WordsWriter;
using Xunit;

namespace WordCounterLibraryTest.WordsWriter
{
  public class FileReporterTest
  {
    [Fact]
    public void Constructor_WhenWordRepositoryIsNull_ThenThrowsArgumentNullException()
    {
      // Arrange
      IWordRepository wordRepository = null!;
      var archiverMock = Substitute.For<IArchiver>();
      var excludedWordsMock = Substitute.For<IExcludedWords>();
      var indexCardsMock = Substitute.For<IIndexCards>();
      var alphabetMock = Substitute.For<IAlphabet>();

      // Act & Assert
      Assert.Throws<ArgumentNullException>(() =>
          new FileReporter(wordRepository, archiverMock, excludedWordsMock, indexCardsMock, alphabetMock));
    }

    [Fact]
    public void Constructor_WhenArchiverIsNull_ThenThrowsArgumentNullException()
    {
      // Arrange
      var wordRepository = Substitute.For<IWordRepository>();
      IArchiver archiverMock = null!;
      var excludedWordsMock = Substitute.For<IExcludedWords>();
      var indexCardsMock = Substitute.For<IIndexCards>();
      var alphabetMock = Substitute.For<IAlphabet>();

      // Act & Assert
      Assert.Throws<ArgumentNullException>(() =>
          new FileReporter(wordRepository, archiverMock, excludedWordsMock, indexCardsMock, alphabetMock));
    }

    [Fact]
    public void Constructor_WhenExcludedWordsIsNull_ThenThrowsArgumentNullException()
    {
      // Arrange
      var wordRepository = Substitute.For<IWordRepository>();
      var archiverMock = Substitute.For<IArchiver>();
      IExcludedWords excludedWordsMock = null!;
      var indexCardsMock = Substitute.For<IIndexCards>();
      var alphabetMock = Substitute.For<IAlphabet>();

      // Act & Assert
      Assert.Throws<ArgumentNullException>(() =>
          new FileReporter(wordRepository, archiverMock, excludedWordsMock, indexCardsMock, alphabetMock));
    }

    [Fact]
    public void Constructor_WhenIndexCardsIsNull_ThenThrowsArgumentNullException()
    {
      // Arrange
      var wordRepository = Substitute.For<IWordRepository>();
      var archiverMock = Substitute.For<IArchiver>();
      var excludedWordsMock = Substitute.For<IExcludedWords>();
      IIndexCards indexCardsMock = null!;
      var alphabetMock = Substitute.For<IAlphabet>();

      // Act & Assert
      Assert.Throws<ArgumentNullException>(() =>
          new FileReporter(wordRepository, archiverMock, excludedWordsMock, indexCardsMock, alphabetMock));
    }

    [Fact]
    public void Constructor_WhenAlphabetIsNull_ThenThrowsArgumentNullException()
    {
      // Arrange
      var wordRepository = Substitute.For<IWordRepository>();
      var archiverMock = Substitute.For<IArchiver>();
      var excludedWordsMock = Substitute.For<IExcludedWords>();
      var indexCardsMock = Substitute.For<IIndexCards>();
      IAlphabet alphabetMock = null!;

      // Act & Assert
      Assert.Throws<ArgumentNullException>(() =>
          new FileReporter(wordRepository, archiverMock, excludedWordsMock, indexCardsMock, alphabetMock));
    }

    [Fact]
    public void WriteReports_WhenCreatingAReportTheIndexCardIsCreatedUsingTheAlphabet_ThenIndexesAreGeneratedBasedOnTheAlphabet()
    {
      // Arrange
      var alphabet = new List<char> { 'a', 'b', 'c' };
      var wordRepositoryMock = Substitute.For<IWordRepository>();
      var archiverMock = Substitute.For<IArchiver>();
      var excludedWordsMock = Substitute.For<IExcludedWords>();
      var indexCardsMock = Substitute.For<IIndexCards>();
      var alphabetMock = Substitute.For<IAlphabet>();
      alphabetMock.Get().Returns(alphabet);

      var fileReporter = new FileReporter(wordRepositoryMock, archiverMock, excludedWordsMock, indexCardsMock, alphabetMock);

      // Act
      fileReporter.WriteReports();

      // Assert
      alphabetMock.Received(1).Get();
      indexCardsMock.Received(3).CreateIndexKey(Arg.Any<char>());
    }

    [Fact]
    public void WriteReports_WhenCreatingAReportArchiveIsCalled_ThenArchiveReceivedOneCall()
    {
      // Arrange
      var alphabet = new List<char> { 'a', 'b', 'c' };
      var wordRepositoryMock = Substitute.For<IWordRepository>();
      var archiverMock = Substitute.For<IArchiver>();
      var excludedWordsMock = Substitute.For<IExcludedWords>();
      var indexCardsMock = Substitute.For<IIndexCards>();
      var alphabetMock = Substitute.For<IAlphabet>();
      alphabetMock.Get().Returns(alphabet);

      var fileReporter = new FileReporter(wordRepositoryMock, archiverMock, excludedWordsMock, indexCardsMock, alphabetMock);

      // Act
      fileReporter.WriteReports();

      // Assert
      archiverMock.Received(1).Archive(indexCardsMock);
    }

    [Fact]
    public void WriteReports_WhenCreatingAReportArchiveExcludedIsCalled_ThenArchiveExcludedReceivedOneCall()
    {
      // Arrange
      var alphabet = new List<char> { 'a', 'b', 'c' };
      var wordRepositoryMock = Substitute.For<IWordRepository>();
      var archiverMock = Substitute.For<IArchiver>();
      var excludedWordsMock = Substitute.For<IExcludedWords>();
      var indexCardsMock = Substitute.For<IIndexCards>();
      var alphabetMock = Substitute.For<IAlphabet>();
      alphabetMock.Get().Returns(alphabet);

      var fileReporter = new FileReporter(wordRepositoryMock, archiverMock, excludedWordsMock, indexCardsMock, alphabetMock);

      // Act
      fileReporter.WriteReports();

      // Assert
      archiverMock.Received(1).ArchiveExcluded(indexCardsMock);
    }
  }
}
