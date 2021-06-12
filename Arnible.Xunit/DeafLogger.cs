using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Arnible.Xunit
{
  [ExcludeFromCodeCoverage]
  public sealed class DeafLogger : ISimpleLogger, IDisposable
  {
    public bool IsLoggerEnabled => false;
    
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