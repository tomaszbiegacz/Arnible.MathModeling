using System;
using System.IO;

namespace Arnible
{
  /// <summary>
  /// The simplest logger api that you can find.
  /// API is NOT thread safe.
  /// </summary>
  public interface ISimpleLogger
  {
    /// <summary>
    /// Does it makes sens to log anything?
    /// </summary>
    bool IsLoggerEnabled { get; }
    
    /// <summary>
    /// If logger is enabled message will be buffered internally so that it can be persisted somewhere at some point.
    /// This is meant to be lightweight operation with O(n) complexity and minimal memory footprint.
    /// </summary>
    void Write(in ReadOnlySpan<char> message);
    
    /// <summary>
    /// If logger is enabled message will be buffered internally so that it can be persisted somewhere at some point.
    /// </summary>
    void Write(MemoryStream message);
  }
}
