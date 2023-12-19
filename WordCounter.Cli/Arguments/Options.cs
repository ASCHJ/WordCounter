using CommandLine;

namespace WordCounter.Cli.Arguments
{
  public class Options
  {
    [Option('d', "directory", Required = true, HelpText = "Input directory path.")]
    public required string DirectoryPath { get; set; }
  }
}
