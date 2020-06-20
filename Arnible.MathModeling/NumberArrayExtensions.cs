using System;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class NumberArrayExtensions
  {
    public static NumberArray ToNumberArray(this IEnumerable<Number> numbers)
    {
      return NumberArray.Create(numbers);
    }

    public static NumberArray ToNumberArray(this IEnumerable<Number> numbers, uint length)
    {
      Number[] items = numbers.ToArray();
      if(items.Length > length)
      {
        throw new ArgumentException(nameof(length));
      }

      uint toAdd = (uint)(length - items.Length);
      return NumberArray.Create(items.Concat(LinqEnumerable.Repeat<Number>(0, toAdd)));
    }

    public static NumberArray ToNumberArray(this IEnumerable<double> numbers)
    {
      return numbers.Select(v => (Number)v).ToNumberArray();
    }

    public static NumberArray ToNumberArray(this IEnumerable<double> numbers, uint length)
    {
      return numbers.Select(v => (Number)v).ToNumberArray(length);
    }

    public static IEnumerable<uint> Indexes(this NumberArray arg)
    {
      return LinqEnumerable.RangeUint(arg.Length);
    }

    public static IEnumerable<uint> IndexesWhere(this NumberArray arg, Func<Number, bool> predicate)
    {
      for (uint i = 0; i < arg.Length; ++i)
      {
        if (predicate(arg[i]))
        {
          yield return i;
        }
      }
    }
  }
}
