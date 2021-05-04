using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class LastExtensions
  {
    public static T Last<T>(this IEnumerable<T> source)
    {
      bool found = false;
#pragma warning disable CS8600
      T result = default;
#pragma warning restore CS8600      
      foreach (T val in source)
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
  }
}