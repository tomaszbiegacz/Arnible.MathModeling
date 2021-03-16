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
    
    /*
     * SingleOrNull 
     */
    
    public static T? SingleOrNone<T>(this IEnumerable<T> source) where T: struct
    {
      bool found = false;
      T? result = default;
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
        return result;
      }
      else
      {
        return null;
      }
    }
    
    public static T? SingleOrNone<T>(in this ReadOnlySpan<T> source) where T: struct
    {
      if(source.Length > 1)
      {
        throw new ArgumentException(nameof(source));
      }
      if(source.Length > 0)
      {
        return source[0];
      }
      else
      {
        return null;
      }
    }
    
    public static T? SingleOrNone<T>(in this Span<T> source) where T: struct
    {
      if(source.Length > 1)
      {
        throw new ArgumentException(nameof(source));
      }
      if(source.Length > 0)
      {
        return source[0];
      }
      else
      {
        return null;
      }
    }
    
    public static T? SingleOrNull<T>(this IEnumerable<T> source) where T: class
    {
      bool found = false;
      T? result = default;
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
        return result;
      }
      else
      {
        return null;
      }
    }
  }
}