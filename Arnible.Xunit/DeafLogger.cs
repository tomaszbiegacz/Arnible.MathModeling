using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Arnible.Xunit
{
  [ExcludeFromCodeCoverage]
  public sealed class DeafLogger : ISimpleLogger, IDisposable
  {
    public bool IsLoggerEnabled => false;
    
    public void Write(in ReadOnlySpan<char> message)
    {
      // intentionally empty
    }

    public void Write(MemoryStream message)
    {
      // intentionally empty
    }
    
    public void Dispose()
    {
      // intentionally empty
    }
  }
}