using Arnible.Assertions;
using Arnible.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Arnible.Test
{
  public class TestsWithLoggerTests
  {
    private readonly ITestOutputHelper _output;
    
    public TestsWithLoggerTests(ITestOutputHelper output)
    {
      _output = output;
    }
    
    private class CustomTests : TestsWithLogger
    {
      public CustomTests(ITestOutputHelper output) : base(output)
      {
        // intentionally empty
      }
    }
    
    [Fact]
    public void ProperTestsSetup()
    {
      using(var tests = new CustomTests(_output))
      {
        var logger = (XunitLogger)tests.Logger;
        
        logger.IsLoggerEnabled.AssertIsTrue();
        tests.DisableLogging();
        logger.IsLoggerEnabled.AssertIsFalse();
        
        logger.IsSavingLogsToFileEnabled.AssertIsFalse();
        tests.BackupLogsToFile();
        logger.IsSavingLogsToFileEnabled.AssertIsTrue();
      }
    }
    
    [Fact]
    public void DeafTestsSetup()
    {
      var skipLoggerSetup = TestsWithLogger.SkipLoggerSetup;
      try
      {
        TestsWithLogger.SkipLoggerSetup = () => true;
        using(var tests = new CustomTests(_output))
        {
          (tests.Logger is DeafLogger).AssertIsTrue();
        }  
      }
      finally
      {
        TestsWithLogger.SkipLoggerSetup = skipLoggerSetup;
      }
    }
  }
}