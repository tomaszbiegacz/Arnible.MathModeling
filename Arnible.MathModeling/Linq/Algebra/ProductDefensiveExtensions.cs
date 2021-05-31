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
    public static T ProductDefensive<T>(this IEnumerable<T> x) where T: IAlgebraUnitRing<T>
    {
      bool isCurrentSet = false;
#pragma warning disable 8600
      T current = default;
#pragma warning restore 8600
      foreach (T v in x)
      {
        if(isCurrentSet)
        {
#pragma warning disable 8602
          current =  current.Multiply(in v);  
#pragma warning restore 8602
        }
        else
        {
          current = v;
          isCurrentSet = true;
        }
      }
      if (isCurrentSet)
      {
#pragma warning disable 8603
        return current;
#pragma warning restore 8603
      }
      else
      {
        throw new ArgumentException("Empty enumerator");
      }
    }
  }
}