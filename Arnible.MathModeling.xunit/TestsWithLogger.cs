using System;
using Arnible.MathModeling.Export;
using Xunit.Abstractions;

namespace Arnible.MathModeling.xunit
{
  public abstract class TestsWithLogger : IDisposable
  {
    private readonly XunitLogger _logger;
    protected TestsWithLogger(ITestOutputHelper output)
    {
      _logger = new XunitLogger(output);
    }

    protected void DisableLogging()
    {
      _logger.IsLoggerEnabled = false;
    }

    protected void BackupLogsToFile()
    {
      _logger.SaveLogsToFile = true;
    }

    protected IMathModelingLogger Logger => _logger;

    public void Dispose()
    {
      _logger.Dispose();
    }
  }
}