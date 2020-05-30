using Arnible.MathModeling.Export;
using Microsoft.Extensions.Logging;
using System;
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
    }

    //
    // ILogger
    //

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
      _output.WriteLine(state.ToString());
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
