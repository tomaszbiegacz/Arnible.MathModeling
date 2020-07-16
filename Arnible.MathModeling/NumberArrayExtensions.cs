using System;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class NumberArrayExtensions
  {
    public static ValueArray<Number> ToValueArray(this IEnumerable<Number> numbers, in uint length)
    {
      ValueArray<Number> items = numbers.ToValueArray();
      if (items.Length > length)
      {
        throw new ArgumentException(nameof(length));
      }

      uint toAdd = length - items.Length;
      return items.Concat(LinqEnumerable.Repeat<Number>(0, toAdd)).ToValueArray();
    }

    public static ValueArray<Number> ToNumberArray(this IEnumerable<double> numbers)
    {
      return numbers.Select(v => (Number)v).ToValueArray();
    }

    public static ValueArray<Number> ToNumberArray(this IEnumerable<double> numbers, in uint length)
    {
      return numbers.Select(v => (Number)v).ToValueArray(in length);
    }    

    public static Number DistanceSquareTo(in this ValueArray<Number> arg, in ValueArray<Number> other)
    {
      if (arg.Length != other.Length)
      {
        throw new ArgumentException(nameof(other));
      }

      return arg.ZipDefensive(other, (a, b) => (a - b).ToPower(2)).SumWithDefault();
    }
  }
}
