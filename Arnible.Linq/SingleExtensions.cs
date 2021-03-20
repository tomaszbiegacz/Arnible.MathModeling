using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class SingleExtensions
  {
    public static T Single<T>(this IEnumerable<T> source)
    {
      bool found = false;
      T result = default;
      foreach(T val in source)
      {
        if(found)
        {
          throw new ArgumentException(nameof(source));
        }
        
        found = true;
        result = val;
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
    
    public static T Single<T>(this IList<T> source)
    {
      if(source.Count == 1)
      {
        return source[0];
      }
      else
      {
        throw new ArgumentException(nameof(source));
      }
    }
    
    public static T Single<T>(this IReadOnlyList<T> source)
    {
      if(source.Count == 1)
      {
        return source[0];
      }
      else
      {
        throw new ArgumentException(nameof(source));
      }
    }
    
    public static ref readonly T Single<T>(in this ReadOnlySpan<T> source) 
    {
      if(source.Length == 1)
      {
        return ref source[0];
      }
      else
      {
        throw new ArgumentException(nameof(source));
      }
    }
    
    public static ref readonly T Single<T>(in this Span<T> source) 
    {
      if(source.Length == 1)
      {
        return ref source[0];
      }
      else
      {
        throw new ArgumentException(nameof(source));
      }
    }
  }
}