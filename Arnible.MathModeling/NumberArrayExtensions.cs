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
      ValueArray<Number> items = numbers.ToValueArray();
      if(items.Length > length)
      {
        throw new ArgumentException(nameof(length));
      }

      uint toAdd = length - items.Length;
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

    public static IEnumerable<uint> Indexes(this IArray<Number> arg)
    {
      return LinqEnumerable.RangeUint(arg.Length);
    }

    public static IEnumerable<uint> IndexesWhere(this IArray<Number> arg, Func<Number, bool> predicate)
    {
      for (uint i = 0; i < arg.Length; ++i)
      {
        if (predicate(arg[i]))
        {
          yield return i;
        }
      }
    }    

    public static Number DistanceSquareTo(this IArray<Number> arg, IArray<Number> other)
    {
      if(arg.Length != other.Length)
      {
        throw new ArgumentException(nameof(other));
      }

      return arg.ZipDefensive(other, (a, b) => (a - b).ToPower(2)).SumWithDefault();
    }
  }
}
