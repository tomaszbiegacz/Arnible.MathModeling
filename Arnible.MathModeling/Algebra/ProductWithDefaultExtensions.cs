using System;
using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  public static class ProductWithDefaultExtensions
  {
    /// <summary>
    /// Calculate items product or return 1 if passed enumerable is empty
    /// </summary>
    public static T ProductWithDefault<T>(this IEnumerable<T> x) where T: struct, IAlgebraUnitRing<T>
    {
      T current = default(T).One;
      foreach (T v in x)
      {
        current =  current.Multiply(in v);
      }
      return current;
    }
  }
}