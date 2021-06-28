using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Arnible.Xunit
{
  [ExcludeFromCodeCoverage]
  public sealed class DeafLogger : ISimpleLoggerForTests
  {
    public bool IsLoggerEnabled => false;
    
    public void EnableLogging(bool value)
    {
      // intentionally empty
    }
    
    public void SaveLogsToFile(bool value)
    {
      // intentionally empty
    }
    
    public ISimpleLogger Write(in ReadOnlySpan<char> message)
    {
      return this;
    }

    public ISimpleLogger Write(MemoryStream message)
    {
      return this;
    }
    
    public void Dispose()
    {
      // intentionally empty
    }
  }
}