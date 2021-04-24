using System;
using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  public static class SumWithDefaultExtensions
  {
    /// <summary>
    /// Calculate items product or return 1 if passed enumerable is empty
    /// </summary>
    public static T SumWithDefault<T>(this IEnumerable<T> x) where T: struct, IAlgebraGroup<T>
    {
      bool anyElement = false;
      T current = default(T).Zero;
      foreach (T v in x)
      {
        current =  current.Add(in v);
        anyElement = true;
      }
      if (!anyElement)
      {
        throw new ArgumentException("Empty enumerator");
      }
      else
      {
        return current;
      }
    }
  }
}