using System.Collections.Generic;
using Arnible.Assertions;

namespace Arnible.MathModeling.Algebra
{
  public static class AddExtensions
  {
    public static T[] Add<T>(this T[] src, IReadOnlyCollection<T> value) where T: struct, IAlgebraGroup<T>
    {
      src.AssertLengthEqualsTo(value);
      ushort pos = 0;
      foreach(T item in value)
      {
        src[pos] = src[pos].Add(in item);
        pos++;
      }
      return src;
    }
  }
}