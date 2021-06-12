using System;
using Arnible.MathModeling;

namespace Arnible
{
  public static class ISimpleLoggerExtensions
  {
    public static ISimpleLogger Write(
      this ISimpleLogger logger, 
      in ReadOnlySpan<char> str0,
      in Number val0)
    {
      Span<char> buffer = stackalloc char[SpanCharFormatter.BufferSize];
      logger.Write(in str0, SpanCharFormatter.ToString((double)val0, in buffer));
      return logger;
    }
    
    public static ISimpleLogger Write(
      this ISimpleLogger logger, 
      in ReadOnlySpan<char> str0,
      in ReadOnlySpan<Number> val0)
    {
      Span<char> buffer = stackalloc char[SpanCharFormatter.BufferSize];
      ReadOnlySpan<char> currentSeparator = ReadOnlySpan<char>.Empty;
      logger.Write(in str0, "[");
      foreach(ref readonly Number v in val0)
      {
        logger.Write(in currentSeparator);
        logger.Write(SpanCharFormatter.ToString((double)v, in buffer));
        currentSeparator = ",";
      }
      logger.Write("]");
      return logger;
    }
  }
}