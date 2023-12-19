using CommandLine;
using CommandLine.Text;

namespace WordCounter.Cli.Arguments
{
  internal class ArgParser
  {
    public static Options Parse(string[] args)
    {
      var parser = new Parser();

      Options? options = null;
      var parserResult = parser.ParseArguments<Options>(args);
      parserResult.WithParsed(o => { options = o; }).WithNotParsed(errs => DisplayHelp(parserResult, errs));
      return parserResult.Value;
    }

    private static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
    {
      var helpText = HelpText.AutoBuild(result, h =>
      {
        h.AdditionalNewLineAfterOption = false;
        h.AutoVersion = false;
        return HelpText.DefaultParsingErrorsHandler(result, h);
      }, e => e);

      Console.WriteLine(helpText);
    }
  }
}
