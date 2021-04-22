using System;
using System.Collections.Generic;

namespace Arnible.Linq.Algebra
{
  public static class ProductDefensiveExtensions
  {
    /// <summary>
    /// Calculate items product or throw ArgumentException if passed enumerable is empty
    /// </summary>
    public static double ProductDefensive(this IEnumerable<double> x)
    {
      bool anyElement = false;
      double current = 1;
      foreach (double v in x)
      {
        current *= v;
        anyElement = true;
      }
      if (!anyElement)
      {
        throw new ArgumentException("Empty enumerator");
      }
      return current;
    }
  }
}