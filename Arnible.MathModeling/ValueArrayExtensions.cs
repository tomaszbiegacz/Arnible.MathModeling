using System;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class ValueArrayExtensions
  {
    public static ValueArray<Number> ToValueArray(this IEnumerable<Number> numbers, uint length)
    {
      ValueArray<Number> items = numbers.ToValueArray();
      if (items.Length > length)
      {
        throw new ArgumentException(nameof(length));
      }

      uint toAdd = length - items.Length;
      return items.GetInternalEnumerable().Concat(LinqEnumerable.Repeat<Number>(0, toAdd)).ToValueArray();
    }

    public static ValueArray<Number> ToValueArray(this ValueArray<Number> numbers, uint length)
    {
      return numbers.GetInternalEnumerable().ToValueArray(length);
    }
    
    //
    // Operators
    //
    
    public static ValueArray<Number> Substract(this ValueArray<Number> a, ValueArray<Number> b)
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
    
    public static ValueArray<Number> Add(this ValueArray<Number> a, ValueArray<Number> b)
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
    
    public static ValueArray<Number> SumDefensive(this IEnumerable<ValueArray<Number>> arg)
    {
      Number[]? result = null;
      foreach (ValueArray<Number> item in arg)
      {
        if (result == null)
        {
          result = System.Linq.Enumerable.ToArray(item.GetInternalEnumerable());
        }
        else
        {
          if (result.Length != item.Length)
          {
            throw new ArgumentException(nameof(arg));
          }
          for (uint i = 0; i < result.Length; ++i)
          {
            result[i] += item[i];
          }
        }
      }

      if (result == null)
      {
        throw new ArgumentException(nameof(arg));
      }
      return result;
    }

    public static ValueArray<Number> Multiply(this ValueArray<Number> arg, in Number value)
    {
      Number[] result = new Number[arg.Length];
      for (uint i = 0; i < arg.Length; ++i)
      {
        result[i] = arg[i] * value;
      }

      return result;
    }
    
    //
    // Operations
    //

    public static Number DistanceSquareTo(this ValueArray<Number> arg, ValueArray<Number> other)
    {
      if (arg.Length != other.Length)
      {
        throw new ArgumentException(nameof(other));
      }

      return arg.GetInternalEnumerable().ZipDefensive(
        col2: other.GetInternalEnumerable(),
        merge: (a, b) => (a - b).ToPower(2)).SumWithDefault();
    }

    public static ValueArray<Number> AddAtPos(this ValueArray<Number> arg, uint pos, in Number value)
    {
      Number[] result = new Number[arg.Length];
      for (uint i = 0; i < arg.Length; ++i)
      {
        result[i] = arg[i];
        if (i == pos)
        {
          result[i] += value;
        }
      }

      return result;
    }
    
    public static ValueArray<Number> SetAtPos(this ValueArray<Number> arg, uint pos, in Number value)
    {
      Number[] result = new Number[arg.Length];
      for (uint i = 0; i < arg.Length; ++i)
      {
        if (i == pos)
        {
          result[i] = value;
        }
        else
        {
          result[i] = arg[i];
        }
      }

      return result;
    }

    public static ValueArray<Number> TranslateCoordinate(
      this ValueArray<Number> start,
      ValueArray<Number> direction,
      in Number value)
    {
      if (start.Length != direction.Length)
      {
        throw new ArgumentException(nameof(direction));
      }
      Number[] result = new Number[start.Length];
      for (uint i = 0; i < start.Length; ++i)
      {
        result[i] = start[i] + direction[i] * value;
      }

      return result;
    }

    //
    // IEnumerable implementation (to avoid boxing)
    //

    public static ValueArray<Number> ExcludeAt(in this ValueArray<Number> x, uint pos)
    {
      return x.GetInternalEnumerable().ExcludeAt(pos).ToValueArray();
    }

    public static Number ProductDefensive(in this ValueArray<Number> arg)
    {
      return arg.GetInternalEnumerable().ProductDefensive();
    }
    
    public static Number SumDefensive(in this ValueArray<Number> arg)
    {
      return arg.GetInternalEnumerable().SumDefensive();
    }
  }
}