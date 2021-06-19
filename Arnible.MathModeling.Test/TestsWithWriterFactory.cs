using System;
using Arnible.Export;
using Arnible.Xunit;
using Xunit.Abstractions;

namespace Arnible.MathModeling.Test
{
  public class TestsWithWriterFactory : IDisposable
  {
    private readonly XunitLogger _logger;
    static readonly IRecordWriterBuilder WriterFactory;
    
    static TestsWithWriterFactory()
    {
      WriterFactory = new RecordWriterBuilder()
        .RegisterMathModellingSerializers();
    }

    public TestsWithWriterFactory(ITestOutputHelper output)
    {
      _logger = new XunitLogger(output);
      Logger = _logger.WithWriterFactory(WriterFactory);
    }
    
    protected void DisableLogging()
    {
      _logger.IsLoggerEnabled = false;
    }

    protected void BackupLogsToFile()
    {
      _logger.SaveLogsToFile = true;
    }

    protected ILoggerWithWriterFactory Logger { get; }
    
    public void Dispose()
    {
      _logger.Dispose();
    }
  }
}