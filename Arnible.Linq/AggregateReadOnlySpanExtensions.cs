using System;

namespace Arnible.Linq
{
  public static class AggregateReadOnlySpanExtensions
  {
    public static double SumWithDefault<T>(in this ReadOnlySpan<T> src, Func<T, double> func)
    {
      double result = 0;
      foreach (T item in src)
      {
        result += func(item);
      }
      return result;
    }
  }
}