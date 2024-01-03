using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using WordCounterLibrary.Format;
using WordCounterLibrary.IO;
using WordCounterLibrary.Managers;
using WordCounterLibrary.Repository;
using WordCounterLibrary.WordsWriter;
using Xunit;

namespace WordCounterLibraryTest.WordsWriter
{
  public class FileArchiverTest
  {
    [Fact]
    public void Construtor_WhenArgumentNull_ThenThrowArgumentNullException()
    {
      // Act / Assert
      Assert.Throws<ArgumentNullException>(() => new FileArchiver(null!, Substitute.For<IWordRepository>(), null!, Substitute.For<IIOManager>(), Substitute.For<IFileWriter>()));
      Assert.Throws<ArgumentNullException>(() => new FileArchiver(new NullLogger<FileArchiver>(), Substitute.For<IWordRepository>(), null!, Substitute.For<IIOManager>(), Substitute.For<IFileWriter>()));
      Assert.Throws<ArgumentNullException>(() => new FileArchiver(new NullLogger<FileArchiver>(), Substitute.For<IWordRepository>(), Substitute.For<IFormatFactory>(), null!, Substitute.For<IFileWriter>()));
      Assert.Throws<ArgumentNullException>(() => new FileArchiver(new NullLogger<FileArchiver>(), Substitute.For<IWordRepository>(), Substitute.For<IFormatFactory>(), Substitute.For<IIOManager>(), null!));
    }

    [Fact]
    public void Archive_WhenArgumentIndexCardsNull_ThenThrowArgumentNullException()
    {
      // Arrange
      var fileArchiver = new FileArchiver(new NullLogger<FileArchiver>(), Substitute.For<IWordRepository>(), Substitute.For<IFormatFactory>(), Substitute.For<IIOManager>(), Substitute.For<IFileWriter>());

      // Act / Assert
      Assert.Throws<ArgumentNullException>(() => fileArchiver.Archive(null!));
    }

    [Fact]
    public void Archive_WhenIndexCardsContainsWord_ThenWriteToFile()
    {
      // Arrange
      var word = "word";
      var wordRepository = Substitute.For<IWordRepository>();
      var formatFactory = Substitute.For<IFormatFactory>();
      formatFactory.CreateFormat<WordAndCountFormat>().Returns(new WordAndCountFormat());
      wordRepository.ElementAtOrDefault(Arg.Any<int>()).Returns(new KeyValuePair<string, int>(word, 1));
      var fileWriter = Substitute.For<IFileWriter>();

      var fileArchiver = new FileArchiver(new NullLogger<FileArchiver>(), wordRepository, formatFactory, Substitute.For<IIOManager>(), fileWriter);

       var indexCards = Substitute.For<IIndexCards>();
       var indexCard = new KeyValuePair<char, HashSet<int>>('W', new HashSet<int> { 1 });
       indexCards.GetIndexCards().Returns(new List<KeyValuePair<char, HashSet<int>>> { indexCard });

      // Act
      fileArchiver.Archive(indexCards);

      // Assert
      fileWriter.Received(1).Write(Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public void Archive_WhenIndexCardsContainsNoWords_ThenWriteToFileIsNotCalled()
    {
      // Arrange
      var wordRepository = Substitute.For<IWordRepository>();
      var formatFactory = Substitute.For<IFormatFactory>();
      formatFactory.CreateFormat<WordAndCountFormat>().Returns(new WordAndCountFormat());
      var fileWriter = Substitute.For<IFileWriter>();

      var fileArchiver = new FileArchiver(new NullLogger<FileArchiver>(), wordRepository, formatFactory, Substitute.For<IIOManager>(), fileWriter);
      var indexCards = Substitute.For<IIndexCards>();
      indexCards.GetIndexCards().Returns(new List<KeyValuePair<char, HashSet<int>>>());

      // Act
      fileArchiver.Archive(indexCards);

      // Assert
      fileWriter.DidNotReceive().Write(Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public void Archive_WhenIndexCardsContainsIndexButNoWord_ThenWriteToFileIsStillCalled()
    {
      // Arrange
      var word = string.Empty;
      var wordRepository = Substitute.For<IWordRepository>();
      var formatFactory = Substitute.For<IFormatFactory>();
      formatFactory.CreateFormat<WordAndCountFormat>().Returns(new WordAndCountFormat());
      wordRepository.ElementAtOrDefault(Arg.Any<int>()).Returns(new KeyValuePair<string, int>(word, 1));
      var fileWriter = Substitute.For<IFileWriter>();

      var fileArchiver = new FileArchiver(new NullLogger<FileArchiver>(), wordRepository, formatFactory, Substitute.For<IIOManager>(), fileWriter);

      var indexCards = Substitute.For<IIndexCards>();
      var indexCard = new KeyValuePair<char, HashSet<int>>('W', new HashSet<int> { 1 });
      indexCards.GetIndexCards().Returns(new List<KeyValuePair<char, HashSet<int>>> { indexCard });

      // Act
      fileArchiver.Archive(indexCards);

      // Assert
      fileWriter.Received().Write(Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public void ArchiveExcluded_WhenArgumentIndexCardsNull_ThenThrowArgumentNullException()
    {
      // Arrange
      var fileArchiver = new FileArchiver(new NullLogger<FileArchiver>(), Substitute.For<IWordRepository>(), Substitute.For<IFormatFactory>(), Substitute.For<IIOManager>(), Substitute.For<IFileWriter>());

      // Act / Assert
      Assert.Throws<ArgumentNullException>(() => fileArchiver.ArchiveExcluded(null!));
    }

    [Fact]
    public void ArchiveExcluded_WhenIndexCardsContainsExcludedWord_ThenWriteToFile()
    {
      // Arrange
      var word = "word";
      var wordRepository = Substitute.For<IWordRepository>();
      var formatFactory = Substitute.For<IFormatFactory>();
      formatFactory.CreateFormat<ReportFormat>().Returns(new ReportFormat());
      wordRepository.ElementAtOrDefault(Arg.Any<int>()).Returns(new KeyValuePair<string, int>(word, 1));
      var fileWriter = Substitute.For<IFileWriter>();

      var fileArchiver = new FileArchiver(new NullLogger<FileArchiver>(), wordRepository, formatFactory, Substitute.For<IIOManager>(), fileWriter);

      var indexCards = Substitute.For<IIndexCards>();
      indexCards.GetExcludedIndexCards().Returns(new List<int> { 1 });

      // Act
      fileArchiver.ArchiveExcluded(indexCards);

      // Assert
      fileWriter.Received(1).Write(Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public void ArchiveExcluded_WhenIndexCardsContainsNoExcludedWords_ThenWriteToFileIsNotCalled()
    {
      // Arrange
      var word = "word";
      var wordRepository = Substitute.For<IWordRepository>();
      var formatFactory = Substitute.For<IFormatFactory>();
      formatFactory.CreateFormat<ReportFormat>().Returns(new ReportFormat());
      wordRepository.ElementAtOrDefault(Arg.Any<int>()).Returns(new KeyValuePair<string, int>(word, 1));
      var fileWriter = Substitute.For<IFileWriter>();

      var fileArchiver = new FileArchiver(new NullLogger<FileArchiver>(), wordRepository, formatFactory, Substitute.For<IIOManager>(), fileWriter);

      var indexCards = Substitute.For<IIndexCards>();
      indexCards.GetExcludedIndexCards().Returns(new List<int>());

      // Act
      fileArchiver.ArchiveExcluded(indexCards);

      // Assert
      fileWriter.DidNotReceive().Write(Arg.Any<string>(), Arg.Any<string>());
    }
  }
}
