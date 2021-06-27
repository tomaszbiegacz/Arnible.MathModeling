using System;
using Xunit.Abstractions;

namespace Arnible.Xunit
{
  public abstract class TestsWithLogger : IDisposable
  {
    private readonly ISimpleLoggerForTests _logger;
    
    public static ISimpleLoggerForTests LoggerForTestsFactory(ITestOutputHelper output)
    {
      if(Environment.GetEnvironmentVariable("arnible_skip_output")?.Trim()?.ToLowerInvariant() != "yes")
        return new XunitLogger(output);
      else
      {
        output.WriteLine("Skipping logger setup");
        return new DeafLogger();
      }
    }
    
    protected TestsWithLogger(ITestOutputHelper output)
    {
      _logger = LoggerForTestsFactory(output);
      Logger = _logger;
    }

    protected void DisableLogging()
    {
      _logger.EnableLogging(false);
    }

    protected void BackupLogsToFile()
    {
      _logger.SaveLogsToFile(true);
    }

    protected ISimpleLogger Logger { get; }

    public void Dispose()
    {
      _logger.Dispose();
    }
  }
}