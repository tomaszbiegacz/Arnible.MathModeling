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
    
    public static void Write(
      this ISimpleLogger logger, 
      in ReadOnlySpan<char> str0,
      in ReadOnlySpan<char> str1)
    {
      logger.Write(in str0);
      logger.Write(in str1);
    }

    public static void Write(
      this ISimpleLogger logger,
      in ReadOnlySpan<char> str0,
      in ReadOnlySpan<char> str1,
      in ReadOnlySpan<char> str2)
    {
      logger.Write(in str0);
      logger.Write(in str1);
      logger.Write(in str2);
    }
    
    public static void Write(
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
    }
    
    public static void Write(
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
    }
    
    public static void Write(
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
    }
    
    public static void Write(
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
    }
    
    public static void Log(this ISimpleLogger logger, string message)
    {
      logger.Write(message);
      logger.Write(_newLine.Span);
    }
    
    public static void Log(
      this ISimpleLogger logger, 
      in ReadOnlySpan<char> str0,
      in ReadOnlySpan<char> str1)
    {
      logger.Write(in str0, in str1);
      logger.Write(_newLine.Span);
    }

    public static void Log(
      this ISimpleLogger logger,
      in ReadOnlySpan<char> str0,
      in ReadOnlySpan<char> str1,
      in ReadOnlySpan<char> str2)
    {
      logger.Write(in str0, in str1, in str2);
      logger.Write(_newLine.Span);
    }
    
    public static void Log(
      this ISimpleLogger logger,
      in ReadOnlySpan<char> str0,
      in ReadOnlySpan<char> str1,
      in ReadOnlySpan<char> str2,
      in ReadOnlySpan<char> str3)
    {
      logger.Write(in str0, in str1, in str2, in str3);
      logger.Write(_newLine.Span);
    }
    
    public static void Log(
      this ISimpleLogger logger,
      in ReadOnlySpan<char> str0,
      in ReadOnlySpan<char> str1,
      in ReadOnlySpan<char> str2,
      in ReadOnlySpan<char> str3,
      in ReadOnlySpan<char> str4)
    {
      logger.Write(in str0, in str1, in str2, in str3, in str4);
      logger.Write(_newLine.Span);
    }
    
    public static void Log(
      this ISimpleLogger logger,
      in ReadOnlySpan<char> str0,
      in ReadOnlySpan<char> str1,
      in ReadOnlySpan<char> str2,
      in ReadOnlySpan<char> str3,
      in ReadOnlySpan<char> str4,
      in ReadOnlySpan<char> str5)
    {
      logger.Write(in str0, in str1, in str2, in str3, in str4, in str5);
      logger.Write(_newLine.Span);
    }
    
    public static void Log(
      this ISimpleLogger logger,
      in ReadOnlySpan<char> str0,
      in ReadOnlySpan<char> str1,
      in ReadOnlySpan<char> str2,
      in ReadOnlySpan<char> str3,
      in ReadOnlySpan<char> str4,
      in ReadOnlySpan<char> str5,
      in ReadOnlySpan<char> str6)
    {
      logger.Write(in str0, in str1, in str2, in str3, in str4, in str5, in str6);
      logger.Write(_newLine.Span);
    }
  }
}