using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class LastOrNullExtensions
  {
    public static T? LastOrNone<T>(this IEnumerable<T> source) where T: struct
    {
      T? result = null;
      foreach(T val in source)
      {
        result = val;
      }
      return result;
    }
    
    public static T? LastOrNone<T>(in this ReadOnlySpan<T> source) where T: struct
    {
      if(source.Length > 0)
      {
        return source[^1];
      }
      else
      {
        return null;
      }
    }
    
    public static T? LastOrNone<T>(in this Span<T> source) where T: struct
    {
      return LastOrNone((ReadOnlySpan<T>)source);
    }
    
    public static T? LastOrNull<T>(this IEnumerable<T> source) where T: class
    {
      T? result = null;
      foreach(T val in source)
      {
        result = val;
      }
      return result;
    }
  }
}