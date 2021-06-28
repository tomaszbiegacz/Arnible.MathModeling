using System;

namespace Arnible.Xunit
{
  public interface ISimpleLoggerForTests : ISimpleLogger, IDisposable
  {
    void EnableLogging(bool value);
    
    void SaveLogsToFile(bool value);
  }
}