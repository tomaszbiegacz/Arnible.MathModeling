using System;
using Arnible.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Arnible.Logger.Test
{
  public class XunitLoggerTests : IDisposable
  {
    private readonly XunitLogger _logger;
    
    public XunitLoggerTests(ITestOutputHelper output)
    {
      _logger = new(output);
    }
    
    [Fact]
    public void InitialValues()
    {
      _logger.Write("something to write");
      
      Assert.True(_logger.IsLoggerEnabled);
      Assert.False(_logger.IsSavingLogsToFileEnabled);
    }
    
    [Fact]
    public void TestDisableLogging()
    {
      _logger.EnableLogging(false);
      _logger.SaveLogsToFile(false);
      _logger.Write("something to write");
      
      Assert.False(_logger.IsLoggerEnabled);
      Assert.False(_logger.IsSavingLogsToFileEnabled);
    }
    
    public void Dispose()
    {
      _logger.Dispose();
    }
  }
}