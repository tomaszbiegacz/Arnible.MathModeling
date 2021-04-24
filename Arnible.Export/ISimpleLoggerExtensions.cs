namespace Arnible.Export
{
  public static class ISimpleLoggerExtensions
  {
    public static ILoggerWithWriterFactory WithWriterFactory(
      this ISimpleLogger logger,
      IRecordWriterBuilder writerFactory)
    {
      return new LoggerWithWriterFactory(logger, writerFactory);
    }
  }
}
