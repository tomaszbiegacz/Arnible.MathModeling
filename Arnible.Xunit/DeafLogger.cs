using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Arnible.Xunit
{
  public sealed class DeafLogger : ISimpleLogger, ILogger, IDisposable
  {
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
      // intentionally empty
    }

    public bool IsLoggerEnabled => false;
    public void Write(in ReadOnlySpan<char> message)
    {
      // intentionally empty
    }

    public void Write(MemoryStream message)
    {
      // intentionally empty
    }

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