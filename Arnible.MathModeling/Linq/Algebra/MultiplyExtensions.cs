using System;
using System.Collections.Generic;
using Arnible.Assertions;

namespace Arnible.MathModeling.Algebra
{
  public static class MultiplyExtensions
  {
    public static T[] Multiply<T>(this IReadOnlyCollection<T> arg, in T value) where T: IAlgebraRing<T>
    {
      T[] result = new T[arg.Count];
      ushort pos = 0;
      foreach (T item in arg)
      {
        result[pos] = item.Multiply(in value);
        pos++;
      }
      return result;
    }

    public static void Multiply<T>(
      in this ReadOnlySpan<T> arg, 
      in T value,
      in Span<T> output) where T: IAlgebraRing<T>
    {
      arg.Length.AssertIsEqualTo(output.Length);
      
      for (ushort i =0; i<arg.Length; ++i)
      {
        output[i] = arg[i].Multiply(in value);
      }
    }
    
    public static void MultiplyInPlace<T>(
      in this Span<T> arg, 
      in T value) where T: IAlgebraRing<T>
    {
      for (ushort i =0; i<arg.Length; ++i)
      {
        arg[i] = arg[i].Multiply(in value);
      }
    }
  }
}