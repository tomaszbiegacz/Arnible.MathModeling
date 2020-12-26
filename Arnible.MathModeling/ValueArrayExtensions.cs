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
    
    //
    // Operators
    //
    
    public static ValueArray<Number> Substract(this in ValueArray<Number> a, in ValueArray<Number> b)
    {
      if (a.Length != b.Length)
      {
        throw new ArgumentException(nameof(a));
      }

      Number[] result = new Number[a.Length];
      for (uint i = 0; i < a.Length; ++i)
      {
        result[i] = a[i] - b[i];
      }
      return new ValueArray<Number>(result);
    }
    
    public static ValueArray<Number> Add(this in ValueArray<Number> a, in ValueArray<Number> b)
    {
      if (a.Length != b.Length)
      {
        throw new ArgumentException(nameof(a));
      }

      Number[] result = new Number[a.Length];
      for (uint i = 0; i < a.Length; ++i)
      {
        result[i] = a[i] + b[i];
      }
      return new ValueArray<Number>(result);
    }
    
    //
    // Operations
    //

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

    public static ValueArray<Number> AddAtPos(in this ValueArray<Number> arg, uint pos, Number value)
    {
      return arg.GetInternalEnumerable().Select((i, v) => i == pos ? v + value : v).ToValueArray();
    }
    
    public static ValueArray<Number> SetAtPos(in this ValueArray<Number> arg, uint pos, Number value)
    {
      return arg.GetInternalEnumerable().Select((i, v) => i == pos ? value : v).ToValueArray();
    }

    public static ValueArray<Number> TranslateCoordinate(
      in this ValueArray<Number> start,
      ValueArray<Number> direction,
      Number value)
    {
      return start.Select((i, v) => v + direction[i] * value).ToValueArray();
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