using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class SumDefensiveExtensions
  {
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
  }
}