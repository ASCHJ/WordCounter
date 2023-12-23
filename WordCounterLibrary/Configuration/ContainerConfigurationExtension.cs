using Autofac;
using Serilog;
using Serilog.Extensions.Autofac.DependencyInjection;
using WordCounterLibrary.Format;
using WordCounterLibrary.IO;
using WordCounterLibrary.LineToWords;
using WordCounterLibrary.Managers;
using WordCounterLibrary.Repository;
using WordCounterLibrary.Services;
using WordCounterLibrary.WordsWriter;

namespace WordCounterLibrary.Configuration
{
  public static class ContainerConfigurationExtension
  {
    internal static ContainerBuilder ConfigureDependencies(this ContainerBuilder builder)
    {
      builder.RegisterType<ExcludeManager>().As<IExcludeManager>();
      builder.RegisterType<IndexCards>().As<IIndexCards>().SingleInstance();
      builder.RegisterType<ExcludedWordsRepository>().As<IExcludedWordsRepository>().SingleInstance();
      builder.RegisterType<LineConsumerService>().As<ILineConsumerService>();
      builder.RegisterType<LineFileProducerService>().As<ILineFileProducerService>();
      builder.RegisterType<ChannelAsBuffer>().As<IBufferStorage>().SingleInstance();
      builder.RegisterType<LineToWordsProcessor>().FindConstructorsWith(type => type.GetDeclaredConstructors()).As<IWordsProcessor>().SingleInstance();
      builder.RegisterType<ConcurrentDictionaryRepository>().As<IWordRepository>().SingleInstance();
      builder.RegisterType<AsciiUpperCaseAlphabet>().As<IAlphabet>();
      builder.RegisterType<FileArchiver>().As<IArchiver>();
      builder.RegisterType<FileReporter>().As<IReporter>();
      builder.RegisterType<ChannelAsBuffer>().FindConstructorsWith(type => type.GetDeclaredConstructors()).As<IBufferStorage>().SingleInstance();

      return builder;
    }

    internal static ContainerBuilder ConfigureIO(this ContainerBuilder builder)
    {
      builder.RegisterType<FileReaderService>().As<IFileReaderService>();
      builder.RegisterType<IOManager>().As<IIOManager>();
      builder.RegisterType<StreamFileWriter>().As<IFileWriter>();
      
      return builder;
    }

    internal static ContainerBuilder ConfigureFormat(this ContainerBuilder builder)
    {
      builder.RegisterType<AsciiUpperCaseAlphabet>().As<IAlphabet>();
      builder.RegisterType<LipsumLineFormatParser>().As<ILineFormatParser>();
      builder.RegisterType<FormatFactory>().As<IFormatFactory>();     

      return builder;
    }

    internal static ContainerBuilder LoggerConfiguration(this ContainerBuilder containerBuilder, LoggerConfiguration loggerConfiguration)
    {
      return containerBuilder.RegisterSerilog(loggerConfiguration);
    }
  }
}
