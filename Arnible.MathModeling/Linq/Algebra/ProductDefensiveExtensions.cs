using System;
using System.Collections.Generic;
using Arnible.MathModeling.Algebra;

namespace Arnible.Linq.Algebra
{
  public static class ProductDefensiveExtensions
  {
    /// <summary>
    /// Calculate items product or throw ArgumentException if passed enumerable is empty
    /// </summary>
    public static T ProductDefensive<T>(this IEnumerable<T> x) where T: struct, IAlgebraUnitRing<T>
    {
      T? current = null;
      foreach (T v in x)
      {
        if(current.HasValue)
        {
          current =  current.Value.Multiply(in v);  
        }
        else
        {
          current = v;
        }
      }
      if (current.HasValue)
      {
        return current.Value;
      }
      else
      {
        throw new ArgumentException("Empty enumerator");
      }
    }
  }
}