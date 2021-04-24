using System;
using System.Collections.Generic;

namespace Arnible.Linq.Algebra
{
  public static class SumWithDefaultExtensions
  {
    /*
     * double
     */
    
    /// <summary>
    /// Calculate items sum or return 0 if passed enumerable is empty
    /// </summary>
    public static double SumWithDefault(this IEnumerable<double> x)
    {
      double current = 0;
      foreach (double v in x)
      {
        current += v;        
      }
      return current;
    }
    
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
    
    /*
     * ushort
     */
    public static uint SumWithDefault(this IEnumerable<ushort> x)
    {
      uint current = 0;
      foreach (ushort v in x)
      {
        current += v;        
      }
      return current;
    }
    
    /*
     * uint
     */
    
    public static ulong SumWithDefault(this IEnumerable<uint> x)
    {
      ulong current = 0;
      foreach (uint v in x)
      {
        current += v;        
      }
      return current;
    }
  }
}