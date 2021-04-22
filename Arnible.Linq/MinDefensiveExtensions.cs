using System;
using System.Collections.Generic;

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
      T result = default;
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
  }
}