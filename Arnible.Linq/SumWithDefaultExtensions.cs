using System;

namespace Arnible.Linq
{
  public static class SumWithDefaultExtensions
  {
    public static double SumWithDefault<T>(in this ReadOnlySpan<T> src, FuncIn<T, double> func)
    {
      double result = 0;
      foreach (ref readonly T item in src)
      {
        result += func(in item);
      }
      return result;
    }
    
    public static double SumWithDefault<T>(in this Span<T> src, FuncIn<T, double> func)
    {
      double result = 0;
      foreach (ref readonly T item in src)
      {
        result += func(in item);
      }
      return result;
    }
  }
}