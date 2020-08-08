using System;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class ValueArrayExtensions
  {
    public static ValueArray<Number> ToValueArray(this IEnumerable<Number> numbers, in uint length)
    {
      ValueArray<Number> items = numbers.ToValueArray();
      if (items.Length > length)
      {
        throw new ArgumentException(nameof(length));
      }

      uint toAdd = length - items.Length;
      return items.GetInternalEnumerable().Concat(LinqEnumerable.Repeat<Number>(0, toAdd)).ToValueArray();
    }

    public static ValueArray<Number> ToValueArray(in this ValueArray<Number> numbers, in uint length)
    {
      return numbers.GetInternalEnumerable().ToValueArray(length);
    }

    public static Number DistanceSquareTo(in this ValueArray<Number> arg, in ValueArray<Number> other)
    {
      if (arg.Length != other.Length)
      {
        throw new ArgumentException(nameof(other));
      }

      return arg.GetInternalEnumerable().ZipDefensive(
        col2: other.GetInternalEnumerable(),
        merge: (a, b) => (a - b).ToPower(2)).SumWithDefault();
    }

    //
    // IEnumerable implementation (to avoid boxing)
    //

    public static IEnumerable<Number> ExcludeAt(in this ValueArray<Number> x, uint pos)
    {
      return x.GetInternalEnumerable().ExcludeAt(pos);
    }

    public static Number ProductDefensive(in this ValueArray<Number> arg)
    {
      return arg.GetInternalEnumerable().ProductDefensive();
    }
  }
}