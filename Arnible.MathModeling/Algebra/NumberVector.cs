using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Arnible.MathModeling.Algebra
{
  /// <summary>
  /// Immutable number vector.
  /// Last dimension in the vector is always either non-zero or zero for 1d vector.
  /// </summary>
  /// <remarks> 
  /// Features:
  /// * Equals returns true only if both arrays have the same size, and elements in each arrays are the same.
  /// * GetHashCode is calculated from array length and each element's hash.
  /// Usage considerations:
  /// * Structure size is equal to IntPtr.Size, hence there is no need to return/receive structure instance by reference
  /// </remarks>  
  [Serializable]
  public readonly struct NumberVector : 
    IEquatable<NumberVector>,
    IComparable<NumberVector>,
    IEquatable<Number>, 
    IValueArray<Number>,
    IValueObject
  {
    readonly static Number ZeroValue = 0d;
    readonly static IReadOnlyCollection<Number> ZeroVector = new Number[] { 0 };

    public static NumberVector Repeat(in Number value, in uint length)
    {
      if (value == 0)
      {
        return default;
      }
      else
      {
        return new NumberVector(LinqEnumerable.Repeat(value, length).ToValueArray());
      }
    }

    public static NumberVector NonZeroValueAt(in uint pos, in Number value)
    {
      if (value == 0)
      {
        throw new ArgumentException(nameof(value));
      }

      return new NumberVector(LinqEnumerable.Repeat<Number>(0, pos).Append(value).ToValueArray());
    }

    public static NumberVector NonZeroValueAt(in IUnmanagedArray<bool> pos, Number value)
    {
      if (value == 0)
      {
        throw new ArgumentException(nameof(value));
      }

      return Create(pos.Select(v => v ? value : 0));
    }

    public static NumberVector NonZeroValueAt(in IUnmanagedArray<Sign> pos, Number value)
    {
      if (value == 0)
      {
        throw new ArgumentException(nameof(value));
      }

      return Create(pos.Select(v => v == Sign.None ? 0 : (int)v * value));
    }

    private readonly ValueArray<Number> _values;

    private static ValueArray<Number> GetNormalizedVector(in IEnumerable<Number> parameters)
    {
      List<Number> result = new List<Number>(parameters ?? LinqEnumerable.Empty<Number>());
      while (result.Count > 0 && result[^1] == 0)
      {
        result.RemoveAt(result.Count - 1);
      }
      return result.ToValueArray();
    }

    internal static NumberVector Create(in IEnumerable<Number> parameters)
    {
      return new NumberVector(GetNormalizedVector(parameters));
    }

    public NumberVector(params Number[] parameters)
      : this(GetNormalizedVector(parameters))
    {
      // intentionally empty
    }

    private NumberVector(ValueArray<Number> parameters)
    {
      _values = parameters;
    }

    public static implicit operator NumberVector(in Number v) => new NumberVector(v);
    public static implicit operator NumberVector(in double v) => new NumberVector(v);

    //
    // Properties
    //            

    public Number GetOrDefault(in uint pos)
    {
      if (pos >= Length)
        return 0;
      else
        return _values[pos];
    }

    //
    // IArray
    //

    internal IEnumerable<Number> GetInternalEnumerable()
    {
      if (_values.Length == 0)
      {
        return ZeroVector;
      }
      else
      {
        return _values.GetInternalEnumerable();
      }
    }

    public ref readonly Number this[in uint pos]
    {
      get
      {
        if (pos == 0 && _values.Length == 0)
        {
          return ref ZeroValue;
        }
        else
        {
          return ref _values[pos];
        }
      }
    }

    public uint Length => Math.Max(1, _values.Length);

    public Number First => GetInternalEnumerable().First();

    public IEnumerator<Number> GetEnumerator() => GetInternalEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetInternalEnumerable().GetEnumerator();

    //
    // IEquatable
    //

    public bool Equals(in NumberVector other) => _values.GetInternalEnumerable().SequenceEqual(other._values.GetInternalEnumerable());

    public bool Equals(NumberVector other) => Equals(in other);

    public bool Equals(in Number other)
    {
      if (Length == 1)
      {
        return this[0] == other;
      }
      else
      {
        return false;
      }
    }

    public bool Equals(Number other) => Equals(in other);

    public override bool Equals(object obj)
    {
      if (obj is NumberVector v)
      {
        return Equals(v);
      }
      else if (obj is Number v2)
      {
        return Equals(in v2);
      }
      else
      {
        return false;
      }
    }

    public string ToString(CultureInfo cultureInfo)
    {
      if (Length == 1)
      {
        return this[0].ToString(cultureInfo);
      }
      else
      {
        return "[" + string.Join(" ", GetInternalEnumerable().Select(v => v.ToString(cultureInfo))) + "]";
      }
    }
    public override string ToString() => ToString(CultureInfo.InvariantCulture);
    public string ToStringValue() => ToString();

    public override int GetHashCode()
    {
      return _values.GetHashCode();
    }
    public int GetHashCodeValue() => GetHashCode();

    public static bool operator ==(in NumberVector a, in NumberVector b) => a.Equals(in b);
    public static bool operator !=(in NumberVector a, in NumberVector b) => !a.Equals(in b);

    public static bool operator ==(in Number a, in NumberVector b) => b.Equals(in a);
    public static bool operator !=(in Number a, in NumberVector b) => !b.Equals(in a);

    public static bool operator ==(in NumberVector a, in Number b) => a.Equals(in b);
    public static bool operator !=(in NumberVector a, in Number b) => !a.Equals(in b);
    
    //
    // IComparable
    //

    public int CompareTo(NumberVector other)
    {
      return GetLengthSquare().CompareTo(other.GetLengthSquare());
    }

    //
    // Query operators
    //

    public Number GetLengthSquare()
    {
      return GetInternalEnumerable().Select(v => v * v).SumWithDefault();
    }

    public NumberVector Transform(in Func<Number, Number> transformation)
    {
      return Create(GetInternalEnumerable().Select(transformation));
    }

    public NumberVector Transform(in Func<uint, Number, Number> transformation)
    {
      return Create(GetInternalEnumerable().Select(transformation));
    }

    //
    // Arithmetic operators
    //

    public static NumberVector operator +(in NumberVector a, in NumberVector b)
    {
      return a.GetInternalEnumerable().Zip(b.GetInternalEnumerable(), (va, vb) => (va ?? 0) + (vb ?? 0)).ToVector();
    }

    public static NumberVector operator -(in NumberVector a, in NumberVector b)
    {
      return a.GetInternalEnumerable().Zip(b.GetInternalEnumerable(), (va, vb) => (va ?? 0) - (vb ?? 0)).ToVector();
    }

    public static NumberVector operator /(in NumberVector a, Number b)
    {
      return new NumberVector(a.GetInternalEnumerable().Select(v => v / b).ToValueArray());
    }

    public static NumberVector operator *(in NumberVector a, Number b)
    {
      if (b == 0)
      {
        return default;
      }
      else
      {
        return new NumberVector(a.GetInternalEnumerable().Select(v => v * b).ToValueArray());
      }
    }

    public static NumberVector operator *(in Number a, in NumberVector b) => b * a;
    
    //
    // IEnumerable extensions (to avoid boxing)
    //

    public bool All(in Func<Number, bool> predicate)
    {
      return GetInternalEnumerable().All(predicate);
    }
    
    public IEnumerable<TResult> Select<TResult>(in Func<Number, TResult> selector)
    {
      return GetInternalEnumerable().Select(selector);
    }

    public Number SumDefensive()
    {
      return GetInternalEnumerable().SumDefensive();
    }

    public IEnumerable<Number> TakeExactly(uint count)
    {
      return GetInternalEnumerable().TakeExactly(count);
    }

    public ValueArray<Number> ToValueArray()
    {
      return _values;
    }
    
    public IEnumerable<Number> Where(in Func<Number, bool> predicate)
    {
      return GetInternalEnumerable().Where(predicate);
    }
  }
}
