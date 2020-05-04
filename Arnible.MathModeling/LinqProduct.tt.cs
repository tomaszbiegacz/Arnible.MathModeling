using System;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class LinqProduct
  {
    public static double ProductDefensive(this IEnumerable<double> x)
    {
      if (x == null)
      {
        throw new ArgumentNullException(nameof(x));
      }
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

    public static double ProductWithDefault(this IEnumerable<double> x)
    {
      if (x == null)
      {
        throw new ArgumentNullException(nameof(x));
      }
      double current = 1;
      foreach (double v in x)
      {
        current *= v;
      }
      return current;
    }
    public static Number ProductDefensive(this IEnumerable<Number> x)
    {
      if (x == null)
      {
        throw new ArgumentNullException(nameof(x));
      }
      bool anyElement = false;
      Number current = 1;
      foreach (Number v in x)
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

    public static Number ProductWithDefault(this IEnumerable<Number> x)
    {
      if (x == null)
      {
        throw new ArgumentNullException(nameof(x));
      }
      Number current = 1;
      foreach (Number v in x)
      {
        current *= v;
      }
      return current;
    }
  }
}
