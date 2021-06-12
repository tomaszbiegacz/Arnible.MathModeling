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
    
    public static ISimpleLogger NewLine(this ISimpleLogger logger)
    {
      logger.Write(_newLine.Span);
      return logger;
    }
    
    public static ISimpleLogger Write(
      this ISimpleLogger logger, 
      in ReadOnlySpan<char> str0,
      in ReadOnlySpan<char> str1)
    {
      logger.Write(in str0);
      logger.Write(in str1);
      return logger;
    }

    public static ISimpleLogger Write(
      this ISimpleLogger logger, 
      in ReadOnlySpan<char> str0,
      in ulong val1)
    {
      Span<char> str1 = stackalloc char[SpanCharFormatter.BufferSize];
      logger.Write(in str0, SpanCharFormatter.ToString(in val1, in str1));
      return logger;
    }

    public static ISimpleLogger Write(
      this ISimpleLogger logger,
      in ReadOnlySpan<char> str0,
      in ReadOnlySpan<char> str1,
      in ReadOnlySpan<char> str2)
    {
      logger.Write(in str0);
      logger.Write(in str1);
      logger.Write(in str2);
      return logger;
    }
    
    public static ISimpleLogger Write(
      this ISimpleLogger logger,
      in ReadOnlySpan<char> str0,
      in ReadOnlySpan<char> str1,
      in ReadOnlySpan<char> str2,
      in ReadOnlySpan<char> str3)
    {
      logger.Write(in str0);
      logger.Write(in str1);
      logger.Write(in str2);
      logger.Write(in str3);
      return logger;
    }
    
    public static ISimpleLogger Write(
      this ISimpleLogger logger,
      in ReadOnlySpan<char> str0,
      in ReadOnlySpan<char> str1,
      in ReadOnlySpan<char> str2,
      in ReadOnlySpan<char> str3,
      in ReadOnlySpan<char> str4)
    {
      logger.Write(in str0);
      logger.Write(in str1);
      logger.Write(in str2);
      logger.Write(in str3);
      logger.Write(in str4);
      return logger;
    }
    
    public static ISimpleLogger Write(
      this ISimpleLogger logger,
      in ReadOnlySpan<char> str0,
      in ReadOnlySpan<char> str1,
      in ReadOnlySpan<char> str2,
      in ReadOnlySpan<char> str3,
      in ReadOnlySpan<char> str4,
      in ReadOnlySpan<char> str5)
    {
      logger.Write(in str0);
      logger.Write(in str1);
      logger.Write(in str2);
      logger.Write(in str3);
      logger.Write(in str4);
      logger.Write(in str5);
      return logger;
    }
    
    public static ISimpleLogger Write(
      this ISimpleLogger logger,
      in ReadOnlySpan<char> str0,
      in ReadOnlySpan<char> str1,
      in ReadOnlySpan<char> str2,
      in ReadOnlySpan<char> str3,
      in ReadOnlySpan<char> str4,
      in ReadOnlySpan<char> str5,
      in ReadOnlySpan<char> str6)
    {
      logger.Write(in str0);
      logger.Write(in str1);
      logger.Write(in str2);
      logger.Write(in str3);
      logger.Write(in str4);
      logger.Write(in str5);
      logger.Write(in str6);
      return logger;
    }
  }
}