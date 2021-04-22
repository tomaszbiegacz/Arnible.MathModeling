using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class MaxOrNullExtensions
  {
    /// <summary>
    /// Finds maximum value or throw ArgumentException if passed enumerable is empty
    /// </summary>
    public static T? MaxOrNone<T>(this IEnumerable<T> x) where T: struct, IComparable<T>
    {
      T? result = null;
      foreach (T v in x)
      {
        if (result is not null)
        {
          if (v.CompareTo(result.Value) > 0)
          {
            result = v;
          }
        }
        else
        {
          result = v;
        }        
      }
      return result;
    } 
    
    /// <summary>
    /// Finds maximum value or throw ArgumentException if passed enumerable is empty
    /// </summary>
    public static T? MaxOrNull<T>(this IEnumerable<T> x) where T: class, IComparable<T>
    {
      T? result = null;
      foreach (T v in x)
      {
        if (result is not null)
        {
          if (v.CompareTo(result) > 0)
          {
            result = v;
          }
        }
        else
        {
          result = v;
        }        
      }
      return result;
    } 
  }
}