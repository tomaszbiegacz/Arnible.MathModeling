using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class FirstExtensions
  {
    public static T First<T>(this IEnumerable<T> source)
    {
      foreach(T val in source)
      {
        return val;
      }
      throw new ArgumentException(nameof(source));
    }
    
    public static T First<T>(this IList<T> source)
    {
      if(source.Count > 0)
      {
        return source[0];
      }
      else
      {
        throw new ArgumentException(nameof(source));
      }
    }
    
    public static T First<T>(this IReadOnlyList<T> source)
    {
      if(source.Count > 0)
      {
        return source[0];
      }
      else
      {
        throw new ArgumentException(nameof(source));
      }
    }
    
    public static ref readonly T First<T>(in this ReadOnlySpan<T> source)
    {
      if(source.Length > 0)
      {
        return ref source[0];
      }
      else
      {
        throw new ArgumentException(nameof(source));
      }
    }
    
    public static ref readonly T First<T>(in this Span<T> source)
    {
      if(source.Length > 0)
      {
        return ref source[0];
      }
      else
      {
        throw new ArgumentException(nameof(source));
      }
    }
    
    /*
     * FirstOrNull
     */

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
      if(source.Length > 0)
      {
        return source[0];
      }
      else
      {
        return null;
      }
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