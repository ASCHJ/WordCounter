using WordCounterLibrary.LineToWords;

namespace WordCounterLibrary.Services
{
  internal interface ILineFileProducerService
  {
    ILineFileProducer Creator();
  }
}
