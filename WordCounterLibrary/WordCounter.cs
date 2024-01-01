using Autofac;
using Microsoft.Extensions.Logging;
using Serilog;
using WordCounterLibrary.Configuration;
using WordCounterLibrary.LineToWords;
using WordCounterLibrary.Managers;
using WordCounterLibrary.WordsWriter;

namespace WordCounterLibrary
{
  public class WordCounter : IDisposable
  {
    private const string _searchPattern = "*.txt";
    private const ushort _producersCount = 4;
    private const ushort _consumersCount = 1;

    private readonly IContainer? _container;
    private readonly ILogger<WordCounter> _logger;
    private readonly IIOManager _iOManager;
    private readonly IWordsProcessor _lineManager;
    private readonly IReporter _reporter;

    public WordCounter(ILogger<WordCounter> logger, IIOManager iOManager, IWordsProcessor lineManager, IReporter reporter)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      _iOManager = iOManager ?? throw new ArgumentNullException(nameof(iOManager));
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
      _iOManager = _container.Resolve<IIOManager>();
      var excludeManager = _container.Resolve<IExcludeManager>();

      var folderWithExcludFile = Path.Combine(_iOManager.CurrentDirectory, "");
      excludeManager.FillExcludeRepositoryWithExcludeWordsFromFile(folderWithExcludFile);

      _lineManager = _container.Resolve<IWordsProcessor>();
      _reporter = _container.Resolve<IReporter>();
    }

    public async Task StartAsync(string directoryPath, CancellationToken cancellationToken)
    {
      string[] filesInDirectory = _iOManager.GetFilesInDirectory(directoryPath, _searchPattern);
      if (filesInDirectory.Any())
      {
        await _lineManager.ExecuteAsync(_producersCount, _consumersCount, filesInDirectory, cancellationToken);
        _reporter.WriteReports();
      }
      else
      {
        _logger.LogInformation("No file found at location '{directory}'", directoryPath);
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
