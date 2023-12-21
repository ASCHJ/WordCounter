using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace WordCounterTest.Helpers
{
  internal class WordCounterCliManager
  {
    private static readonly string _fileName = "WordCounter.Cli.exe";

    public static int ExecuteByCommandLine(string directoryPathInputByCommandline, ITestOutputHelper testOutputHelper)
    {
      var processStartInfo = CreateProcessStartInfo(directoryPathInputByCommandline);

      int exitCode;
      using (var process = Process.Start(processStartInfo))
      {
        if (process == null) { Assert.Fail("Something went wrong when starting the WordCounter program"); };
        process.OutputDataReceived += (sender, e) =>
        {
          if (e.Data != null)
          {
            testOutputHelper.WriteLine(e.Data);
          }
        };

        process.BeginOutputReadLine();
        process.WaitForExit();

        exitCode = process.ExitCode;
      }

      return exitCode;
    }

    private static ProcessStartInfo CreateProcessStartInfo(string arguments)
    {
      return new ProcessStartInfo
      {
        FileName = Path.Combine(Directory.GetCurrentDirectory(), _fileName),
        Arguments = arguments,
        RedirectStandardOutput = true,
        UseShellExecute = false,
        CreateNoWindow = true
      };
    }
  }
}
