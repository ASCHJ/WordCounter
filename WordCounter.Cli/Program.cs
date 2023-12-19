using Microsoft.Extensions.Logging;
using Serilog;
using WordCounter.Cli.Arguments;

class Program
{
  private static ILogger<Program>? logger;
  static async Task<int> Main(string[] args)
  {
    var cancellationTokenSource = new CancellationTokenSource();

    try
    {
      Log.Logger = new LoggerConfiguration()
          .MinimumLevel.Information()
          .WriteTo.Console()
          .CreateLogger();

      using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
      {
        builder.AddSerilog();
      });

      logger = loggerFactory.CreateLogger<Program>();
      logger.LogInformation("WordCounter is started");

      var options = ArgParser.Parse(args);
      if (options == null)
      {
        return -1;
      }

      var validDirectoryStatus = ValidDirectory(options.DirectoryPath);
      if (validDirectoryStatus != 0)
      {
        return validDirectoryStatus;
      }

      var wordCounter = new WordCounterLibrary.WordCounter();

      logger.LogInformation("WordCounter is started");
      await wordCounter.StartAsync(options.DirectoryPath, cancellationTokenSource.Token);

      return 0;
    }
    catch (Exception ex)
    {
      logger?.LogCritical(ex, "Unhandled exception");

      cancellationTokenSource.Cancel();
      return -1;
    }
    finally
    {
      Log.CloseAndFlush();
    }
  }

  private static int ValidDirectory(string inputDirectoryPath)
  {
    if (Directory.Exists(inputDirectoryPath))
    {
      return 0;
    }
    else
    {
      Console.WriteLine($"Directory '{inputDirectoryPath}' is not valid or does not exists");
      return -1;
    }
  }
}
