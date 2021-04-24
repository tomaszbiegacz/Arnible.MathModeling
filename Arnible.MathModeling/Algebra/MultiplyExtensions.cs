using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  public static class MultiplyExtensions
  {
    public static T[] Multiply<T>(this IReadOnlyCollection<T> arg, in T value) where T: struct, IAlgebraRing<T>
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
  }
}