using System;

namespace Arnible
{
  public static class ISimpleLoggerExtensions
  {
    static readonly ReadOnlyMemory<char> _newLine;
    
    static ISimpleLoggerExtensions()
    {
      _newLine = Environment.NewLine.AsMemory();
    }
    
    public static void Log(this ISimpleLogger logger, string message)
    {
      logger.Write(message.AsMemory().Span);
      logger.Write(_newLine.Span);
    }
  }
}