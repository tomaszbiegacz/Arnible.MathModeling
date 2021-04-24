using System;
using System.Collections.Generic;

namespace Arnible.Linq.Algebra
{
  public static class ProductWithDefaultExtensions
  {
    /// <summary>
    /// Calculate items product or return 1 if passed enumerable is empty
    /// </summary>
    public static double ProductWithDefault(this IEnumerable<double> x)
    {
      double current = 1;
      foreach (double v in x)
      {
        current *= v;
      }
      return current;
    }
  }
}