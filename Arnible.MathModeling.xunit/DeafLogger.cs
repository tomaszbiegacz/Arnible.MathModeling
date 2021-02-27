using System;
using Arnible.MathModeling.Export;
using Microsoft.Extensions.Logging;

namespace Arnible.MathModeling.xunit
{
  public sealed class DeafLogger : IMathModelingLogger, ILogger, IDisposable
  {
    public void Log(string message)
    {
      // intentionally empty
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
      // intentionally empty
    }

    public bool IsLoggerEnabled => false;
    public bool IsEnabled(LogLevel logLevel) => false;

    public IDisposable BeginScope<TState>(TState state)
    {
      return this;
    }

    public void Dispose()
    {
      // intentionally empty
    }
  }
}