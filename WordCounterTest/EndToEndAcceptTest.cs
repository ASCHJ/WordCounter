using WordCounterTest.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace WordCounterLibraryTest
{
  public class EndToEndAcceptTest
  {
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly string _currentDirectory;
    private readonly string _folderWithTestFiles;
    private readonly IReadOnlyList<string> _expectedTestFiles;
    private readonly IReadOnlyList<string> _expectedOutputFileNames;
    private readonly string _expectedExcludeReportName;

    public EndToEndAcceptTest(ITestOutputHelper testOutputHelper)
    {
      _testOutputHelper = testOutputHelper;

      _currentDirectory = Directory.GetCurrentDirectory();
      _folderWithTestFiles = Path.Combine(_currentDirectory, "TestData");
      _expectedTestFiles = CreateExpectedTestFilesPaths(_folderWithTestFiles, new List<string> { "200.txt", "300.txt", "400.txt", "500.txt" });

      _expectedOutputFileNames = new List<string>
        {
            "FILE_A.txt", "FILE_B.txt", "FILE_C.txt", "FILE_D.txt", "FILE_E.txt",
            "FILE_F.txt", "FILE_G.txt", "FILE_H.txt", "FILE_I.txt", "FILE_J.txt",
            "FILE_K.txt", "FILE_L.txt", "FILE_M.txt", "FILE_N.txt", "FILE_O.txt",
            "FILE_P.txt", "FILE_Q.txt", "FILE_R.txt", "FILE_S.txt", "FILE_T.txt",
            "FILE_U.txt", "FILE_V.txt", "FILE_W.txt", "FILE_X.txt", "FILE_Y.txt", "FILE_Z.txt"
        };

      _expectedExcludeReportName = "ExcludeReport.txt";
    }

    [Fact]
    public void WordCounterCli_WhenExecutedCliProgram_ThenAllAcceptioncriteriasAreMet()
    {
      // Arrange
      // Check if preconditions are met
      EnsureTestFilesExist(_folderWithTestFiles, _expectedTestFiles);
      DeleteGeneratedFiles(_expectedOutputFileNames, _expectedExcludeReportName);
      CopyTestDataToExcludeFile("exclude.test.txt", "exlude.txt");

      // Cli Argument
      var directoryPathByCommandline = $"--directory {_folderWithTestFiles}";

      // Act
      var exitCode = WordCounterCliManager.ExecuteByCommandLine(directoryPathByCommandline, _testOutputHelper);

      // Assert
      // Create a file for each letter in the alphabet
      // File format is FILE_<letter>.txt
      ValidateExpectedFiles(_expectedOutputFileNames, "FILE_?.txt"); // FILE_<letter>.txt. Note "?" can be anything

      // Have a exlude file that contains the excluded words
      // Exclude 10 words
      ValidateExcludedFile("exlude.txt", 10);

      // Assert file content for output files and exclude report matches expectations
      int wordCount = GetWordCountFromMatchingPattern(_expectedOutputFileNames,    @"\s{2}\w+\s(\d+)$"); // "  <WORD> <number>"
      var wordExcludedCount = GetWordCountFromMatchingPattern(_expectedExcludeReportName, @"^\w+\s(\d+)$");     // "<WORD> <number>"
      Assert.Equal((200 + 300 + 400 + 500), wordCount + wordExcludedCount);                              // Test file are named after how many words they contain (<number>.txt)

      // Assert case insensitive.
      var count = NumberOfWordOccurrencesInCaseInsensitiveSearch("FILE_A.txt", "Aliquam");                // in "200.txt" aliquam is present as "Aliquam" and "aliquam"
      Assert.Equal(1, count);

      // Assert excluded words not saved in output files
      var excludedWord = "sodales";
      ValidateWordIsNotInFile(excludedWord, "FILE_S.txt");
      ValidateWordIsInFile(excludedWord, "exlude.txt");

      //Assert exit code
      Assert.Equal(0, exitCode);
    }

    private void DeleteGeneratedFiles(IEnumerable<string> outputFileNames, string expectedExcludeReportName)
    {
      foreach(var file in outputFileNames)
      {
        File.Delete(Path.Combine(_currentDirectory, file));
      }
      File.Delete(Path.Combine(_currentDirectory, expectedExcludeReportName));
    }

    private void ValidateWordIsInFile(string excludedWord, string filename)
    {
      var occurenceOfExcludedWordInReportFile = FileManagement.SearchForCaseInsensitiveWord(Path.Combine(_currentDirectory, filename), excludedWord);
      Assert.Equal(1, occurenceOfExcludedWordInReportFile);
    }

    private void ValidateWordIsNotInFile(string excludedWord, string filename)
    {
      var occurenceOfExcludedWordInOutputfile = FileManagement.SearchForCaseInsensitiveWord(Path.Combine(_currentDirectory, filename), excludedWord);
      Assert.Equal(0, occurenceOfExcludedWordInOutputfile);
    }

    private int GetWordCountFromMatchingPattern(string fileName, string pattern)
    {
      return FileManagement.GetWordCountFromMatchingPattern(new List<string> { fileName }, pattern);
    }

    private int GetWordCountFromMatchingPattern(IEnumerable<string> fileNames, string pattern)
    {
      var paths = fileNames.Select(fileName => Path.Combine(_currentDirectory, fileName));
      return FileManagement.GetWordCountFromMatchingPattern(paths, pattern);
    }

    private static void EnsureTestFilesExist(string folderPath, IEnumerable<string> fileNames)
    {
      Assert.True(Directory.Exists(folderPath), "Folder with test files does not exist");
      var actualTxtFilesInDirectory = Directory.GetFiles(folderPath, "*.txt");
      Assert.Equivalent(fileNames, actualTxtFilesInDirectory);
    }

    private static void CopyTestDataToExcludeFile(string excludeTestFile, string excludeFile)
    {
      FileManagement.FileContentCopy(Path.Combine(Directory.GetCurrentDirectory(), excludeTestFile), Path.Combine(Directory.GetCurrentDirectory(), excludeFile));
    }

    private void ValidateExpectedFiles(IEnumerable<string> expectedFiles, string searchPattern)
    {
      var filesMatchingExpectedFileFormat = Directory.GetFiles(_currentDirectory, searchPattern);
      var expectedOutputFiles = expectedFiles.Select(file => Path.Combine(_currentDirectory, file));

      Assert.Equivalent(expectedOutputFiles, filesMatchingExpectedFileFormat);
    }

    private void ValidateExcludedFile(string excludedFileName, int expectedExcludedWordCount)
    {
      var excludeFilePath = Path.Combine(_currentDirectory, excludedFileName);
      Assert.True(File.Exists(excludeFilePath));

      var excludedFileContent = File.ReadAllLines(excludeFilePath); //TODO Like a minor since the exclude file does not grow as much as the other files can do.
      Assert.Equal(expectedExcludedWordCount, excludedFileContent.Length);
    }

    private int NumberOfWordOccurrencesInCaseInsensitiveSearch(string fileName, string word)
    {
      return FileManagement.SearchForCaseInsensitiveWord(Path.Combine(_currentDirectory, fileName), word);
    }

    private static List<string> CreateExpectedTestFilesPaths(string folderWithTestFiles, IEnumerable<string> fileNames)
    {
      List<string> pathAndFileName = new();
      foreach (var file in fileNames)
      {
        pathAndFileName.Add(Path.Combine(folderWithTestFiles, file));
      }

      return pathAndFileName;
    }
  }
}
