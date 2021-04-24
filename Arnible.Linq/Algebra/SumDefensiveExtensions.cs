using System;
using System.Collections.Generic;

namespace Arnible.Linq.Algebra
{
  public static class SumDefensiveExtensions
  {
    /*
     * double
     */
    
    /// <summary>
    /// Calculate items sum or throw ArgumentException if passed enumerable is empty
    /// </summary>
    public static double SumDefensive(this IEnumerable<double> x)
    {      
      bool anyElement = false;
      double current = 0;
      foreach (double v in x)
      {
        current += v;
        anyElement = true;
      }
      if (!anyElement)
      {
        throw new ArgumentException("Empty enumerator");
      }
      return current;
    }
    
    /*
     * ushort
     */
    
    public static uint SumDefensive(this IEnumerable<ushort> x)
    {      
      bool anyElement = false;
      uint current = 0;
      foreach (ushort v in x)
      {
        current += v;
        anyElement = true;
      }
      if (!anyElement)
      {
        throw new ArgumentException("Empty enumerator");
      }
      return current;
    }
    
    public static uint SumDefensive<T>(this IEnumerable<T> x, Func<T, ushort> func)
    {      
      return x.Select(func).SumDefensive();
    }
    
    /*
     * uint
     */
    
    public static ulong SumDefensive(this IEnumerable<uint> x)
    {      
      bool anyElement = false;
      ulong current = 0;
      foreach (uint v in x)
      {
        current += v;
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