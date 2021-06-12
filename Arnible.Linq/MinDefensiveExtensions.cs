using System;
using System.Collections.Generic;
using Arnible.Assertions;

namespace Arnible.Linq
{
  public static class MinDefensiveExtensions
  {
    /// <summary>
    /// Finds minimum value or throw ArgumentException if passed enumerable is empty
    /// </summary>
    public static T MinDefensive<T>(this IEnumerable<T> x) where T: IComparable<T>
    {      
      bool isResultKnown = false;
#pragma warning disable CS8600
      T result = default;
#pragma warning restore CS8600
      foreach (T v in x)
      {
        if (isResultKnown)
        {
          if (v.CompareTo(result) < 0)
          {
            result = v;
          }
        }
        else
        {
          result = v;
          isResultKnown = true;
        }        
      }
      
      if (isResultKnown)
      {
        return result!;
      }
      else
      {
        throw new ArgumentException("Empty enumerator");
      }
    }
    
    /// <summary>
    /// Finds maximum value or throw ArgumentException if passed enumerable is empty
    /// </summary>
    public static ref readonly T MinDefensive<T>(this in ReadOnlySpan<T> x) where T: IComparable<T>
    {      
      x.Length.AssertIsGreaterThan(0);
      ref readonly T result = ref x[0];
      for(ushort i=1; i<x.Length; ++i)
      {
        if (x[i].CompareTo(result) < 0)
        {
          result = ref x[i];
        }
      }
      return ref result;
    }
    
    public static ref readonly T MinDefensive<T>(this in Span<T> x) where T: IComparable<T>
    {
      x.Length.AssertIsGreaterThan(0);
      ref readonly T result = ref x[0];
      for(ushort i=1; i<x.Length; ++i)
      {
        if (x[i].CompareTo(result) < 0)
        {
          result = ref x[i];
        }
      }
      return ref result;
    }
  }
}