using System;
using Xunit.Abstractions;

namespace Arnible.Xunit
{
  public abstract class TestsWithLogger : IDisposable
  {
    private readonly XunitLogger _logger;
    protected TestsWithLogger(ITestOutputHelper output)
    {
      _logger = new XunitLogger(output);
      Logger = _logger;
    }

    protected void DisableLogging()
    {
      _logger.IsLoggerEnabled = false;
    }

    protected void BackupLogsToFile()
    {
      _logger.SaveLogsToFile = true;
    }

    protected ISimpleLogger Logger { get; }

    public void Dispose()
    {
      _logger.Dispose();
    }
  }
}