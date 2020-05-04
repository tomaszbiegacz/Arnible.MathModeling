using System;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class LinqSum
  {
    public static double SumDefensive(this IEnumerable<double> x)
    {
      if (x == null)
      {
        throw new ArgumentNullException(nameof(x));
      }
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

    public static double SumWithDefault(this IEnumerable<double> x)
    {
      if (x == null)
      {
        throw new ArgumentNullException(nameof(x));
      }      
      double current = 0;
      foreach (double v in x)
      {
        current += v;        
      }
      return current;
    }        
    
    public static Number SumDefensive(this IEnumerable<Number> x)
    {
      if (x == null)
      {
        throw new ArgumentNullException(nameof(x));
      }
      bool anyElement = false;
      Number current = 0;
      foreach (Number v in x)
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

    public static Number SumWithDefault(this IEnumerable<Number> x)
    {
      if (x == null)
      {
        throw new ArgumentNullException(nameof(x));
      }      
      Number current = 0;
      foreach (Number v in x)
      {
        current += v;        
      }
      return current;
    }        
    
    public static long SumDefensive(this IEnumerable<int> x)
    {
      if (x == null)
      {
        throw new ArgumentNullException(nameof(x));
      }
      bool anyElement = false;
      long current = 0;
      foreach (int v in x)
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

    public static long SumWithDefault(this IEnumerable<int> x)
    {
      if (x == null)
      {
        throw new ArgumentNullException(nameof(x));
      }      
      long current = 0;
      foreach (int v in x)
      {
        current += v;        
      }
      return current;
    }        
    
    public static ulong SumDefensive(this IEnumerable<uint> x)
    {
      if (x == null)
      {
        throw new ArgumentNullException(nameof(x));
      }
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

    public static ulong SumWithDefault(this IEnumerable<uint> x)
    {
      if (x == null)
      {
        throw new ArgumentNullException(nameof(x));
      }      
      ulong current = 0;
      foreach (uint v in x)
      {
        current += v;        
      }
      return current;
    }        
    
  }
}
