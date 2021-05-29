using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class SingleOrNullExtensions
  {
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
      return SingleOrNone((ReadOnlySpan<T>)source);
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