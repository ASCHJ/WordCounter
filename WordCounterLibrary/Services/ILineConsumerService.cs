using WordCounterLibrary.LineToWords;

namespace WordCounterLibrary.Services
{
  internal interface ILineConsumerService
  {
    ILineConsumer Creator();
  }
}
