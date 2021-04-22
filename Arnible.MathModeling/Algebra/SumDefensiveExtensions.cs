using System;
using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  public static class SumDefensiveExtensions
  {
    /// <summary>
    /// Calculate items sum or throw ArgumentException if passed enumerable is empty
    /// </summary>
    public static T SumDefensive<T>(this IEnumerable<T> x) where T: struct, IAlgebraGroup<T>
    {
      T? current = null;
      foreach (T v in x)
      {
        if(current.HasValue)
        {
          current =  current.Value.Add(in v);  
        }
        else
        {
          current = v;
        }
      }
      if (current.HasValue)
      {
        return current.Value;
      }
      else
      {
        throw new ArgumentException("Empty enumerator");
      }
    }
  }
}