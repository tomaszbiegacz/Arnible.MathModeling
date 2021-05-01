using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class MaxDefensiveExtensions
  {
    /// <summary>
    /// Finds maximum value or throw ArgumentException if passed enumerable is empty
    /// </summary>
    public static T MaxDefensive<T>(this IEnumerable<T> x) where T: IComparable<T>
    {      
      bool isResultKnown = false;
#pragma warning disable CS8600
      T result = default;
#pragma warning restore CS8600
      foreach (T v in x)
      {
        if (isResultKnown)
        {
          if (v.CompareTo(result) > 0)
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
    public static T MaxDefensive<T>(this ReadOnlyArray<T> x) where T: IComparable<T>, IEquatable<T>
    {      
      return x.AsList().MaxDefensive();
    }
  }
}