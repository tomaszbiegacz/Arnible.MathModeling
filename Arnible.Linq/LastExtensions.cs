using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class LastExtensions
  {
    public static T Last<T>(this IEnumerable<T> source)
    {
      bool found = false;
      T result = default;
      
      foreach(T val in source)
      {
        result = val;
        found = true;
      }
      
      if(found)
      {
        return result ?? throw new InvalidOperationException("Something went wrong");
      }
      else
      {
        throw new ArgumentException(nameof(source));
      }
    }
    
    public static T Last<T>(this IList<T> source)
    {
      if(source.Count > 0)
      {
        return source[^1];
      }
      else
      {
        throw new ArgumentException(nameof(source));
      }
    }
    
    public static T Last<T>(this IReadOnlyList<T> source)
    {
      if(source.Count > 0)
      {
        return source[^1];
      }
      else
      {
        throw new ArgumentException(nameof(source));
      }
    }
    
    public static ref readonly T Last<T>(in this ReadOnlySpan<T> source) 
    {
      if(source.Length > 0)
      {
        return ref source[^1];
      }
      else
      {
        throw new ArgumentException(nameof(source));
      }
    }
    
    public static ref readonly T Last<T>(in this Span<T> source) 
    {
      if(source.Length > 0)
      {
        return ref source[^1];
      }
      else
      {
        throw new ArgumentException(nameof(source));
      }
    }
    
    /*
     * LastOrNull
     */
    
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
      if(source.Length > 0)
      {
        return source[^1];
      }
      else
      {
        return null;
      }
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