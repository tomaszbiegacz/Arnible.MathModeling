using Arnible.MathModeling.Export;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using Xunit.Abstractions;

namespace Arnible.MathModeling
{
  public sealed class XunitLogger : IMathModelingLogger, ILogger, IDisposable
  {
    private readonly ITestOutputHelper _output;

    public XunitLogger(ITestOutputHelper output)
    {
      _output = output;
    }

    //
    // IMathModelingLogger
    //

    public void Log(string message)
    {
      _output.WriteLine(message);
      Debug.WriteLine(message);
    }

    //
    // ILogger
    //

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
      Log(state.ToString());
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable BeginScope<TState>(TState state) => this;

    //
    // IDisposable
    //

    public void Dispose()
    {
      // do nothing
    }
  }
}
