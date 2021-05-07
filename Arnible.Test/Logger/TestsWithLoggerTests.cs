using Arnible.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Arnible.Logger.Test
{
  public class TestsWithLoggerTests : TestsWithLogger
  {
    public TestsWithLoggerTests(ITestOutputHelper output) : base(output)
    {
      // intentionally empty
    }
    
    [Fact]
    public void InitialValues()
    {
      Logger.Log("something to write");
      
      XunitLogger logger = (XunitLogger)Logger; 
      Assert.True(logger.IsLoggerEnabled);
      Assert.False(logger.SaveLogsToFile);
    }
    
    [Fact]
    public void TestDisableLogging()
    {
      DisableLogging();
      Logger.Log("something to write");
      
      XunitLogger logger = (XunitLogger)Logger;
      Assert.False(logger.IsLoggerEnabled);
      Assert.False(logger.SaveLogsToFile);
    }
    
    [Fact]
    public void TestBackupLogsToFile()
    {
      BackupLogsToFile();
      Logger.Log("something to write");
      
      XunitLogger logger = (XunitLogger)Logger;
      Assert.True(logger.IsLoggerEnabled);
      Assert.True(logger.SaveLogsToFile);
    }
  }
}