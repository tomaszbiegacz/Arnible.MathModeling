using System;
using Xunit.Abstractions;

namespace Arnible.Xunit
{
  public abstract class TestsWithLogger : IDisposable
  {
    internal static Func<bool> SkipLoggerSetup { get; set; } = () => Environment.GetEnvironmentVariable("arnible_skip_output")?.Trim()?.ToLowerInvariant() == "yes";
    private readonly ISimpleLoggerForTests _logger;
    
    public static ISimpleLoggerForTests LoggerForTestsFactory(ITestOutputHelper output)
    {
      if(!SkipLoggerSetup())
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

    public void DisableLogging()
    {
      _logger.EnableLogging(false);
    }

    public void BackupLogsToFile()
    {
      _logger.SaveLogsToFile(true);
    }

    public ISimpleLogger Logger { get; }

    public void Dispose()
    {
      _logger.Dispose();
    }
  }
}