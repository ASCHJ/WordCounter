using Autofac;
using Microsoft.Extensions.Logging;
using Serilog;
using WordCounterLibrary.Configuration;
using WordCounterLibrary.Helpers;
using WordCounterLibrary.LineToWords;
using WordCounterLibrary.WordsWriter;

namespace WordCounterLibrary
{
  public class WordCounter : IDisposable
  {
    private readonly IContainer? _container;
    private readonly ILogger<WordCounter> _logger;
    private readonly IWordsProcessor _lineManager;
    private readonly IReporter _reporter;

    public WordCounter(ILogger<WordCounter> logger, IWordsProcessor lineManager, IReporter reporter)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _lineManager = lineManager ?? throw new ArgumentNullException(nameof(lineManager));
      _reporter = reporter ?? throw new ArgumentNullException(nameof(reporter));
    }

    public WordCounter()
    {
      var containerBuilder = new ContainerBuilder();
      containerBuilder.ConfigureDependencies();
      containerBuilder.ConfigureFormat();
      containerBuilder.ConfigureIO();

      var configuration = new LoggerConfiguration() //TODO configure by configuration file
     .Enrich.FromLogContext()
     .MinimumLevel.Information()
     .WriteTo.Console();

      containerBuilder.LoggerConfiguration(configuration);
      _container = containerBuilder.Build();

      _logger = _container.Resolve<ILogger<WordCounter>>();
      var excludedWords = _container.Resolve<IExcludedWords>();
      var iOManager = _container.Resolve<IIOManager>();

      var folderWithExcludFile = Path.Combine(iOManager.CurrentDirectory, "");
      excludedWords.ReadExcludedWords(folderWithExcludFile);

      _lineManager = _container.Resolve<IWordsProcessor>();
      _reporter = _container.Resolve<IReporter>();
    }

    public async Task StartAsync(string directoryPath, CancellationToken cancellationToken)
    {
      int producersCount = 4;
      int consumersCount = 1;

      string[] filesInDir = GetFilesInDirectory(directoryPath);
      if (filesInDir.Any())
      {
        await _lineManager.Execute(producersCount, consumersCount, filesInDir, cancellationToken);
        _reporter.WriteReports();
      }
      else
      {
        _logger.LogInformation("No file found at location '{directory}'", directoryPath);
      }
    }

    private static string[] GetFilesInDirectory(string directoryPath) //TODO: Don't use directly - also handle throws exceptions
    {
      if (Directory.Exists(directoryPath))
      {
        return Directory.GetFiles(directoryPath, "*.txt"); 
      }
      else
      {
        return Array.Empty<string>();
      }  
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        _container?.Dispose();
      }
    }
  }
}
