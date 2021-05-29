using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class FirstOrNullExtensions
  {
    public static T? FirstOrNone<T>(this IEnumerable<T> source) where T: struct
    {
      foreach(T val in source)
      {
        return val;
      }
      return null;
    }
    
    public static T? FirstOrNone<T>(in this ReadOnlySpan<T> source) where T: struct
    {
      if(source.Length > 0)
      {
        return source[0];
      }
      else
      {
        return null;
      }
    }
    
    public static T? FirstOrNone<T>(in this Span<T> source) where T: struct
    {
      return FirstOrNone((ReadOnlySpan<T>)source);
    }
    
    public static T? FirstOrNull<T>(this IEnumerable<T> source) where T: class
    {
      foreach(T val in source)
      {
        return val;
      }
      return null;
    }
  }
}