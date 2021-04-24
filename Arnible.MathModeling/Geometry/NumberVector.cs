using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Arnible.Linq;
using Arnible.Linq.Algebra;
using Arnible.MathModeling.Algebra;

namespace Arnible.MathModeling.Geometry
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
    IEquatable<Number>
  {
    readonly static Number ZeroValue = 0d;
    readonly static IReadOnlyCollection<Number> ZeroVector = new Number[] { 0 };

    public static NumberVector Repeat(in Number value, uint length)
    {
      if (value == 0)
      {
        return default;
      }
      else
      {
        return new NumberVector(LinqEnumerable.Repeat(value, length).ToArray());
      }
    }

    public static NumberVector NonZeroValueAt(uint pos, in Number value)
    {
      if (value == 0)
      {
        throw new ArgumentException(nameof(value));
      }

      return new NumberVector(LinqEnumerable.Repeat<Number>(0, pos).Append(value).ToArray());
    }

    private readonly ReadOnlyArray<Number> _values;

    private static ReadOnlyArray<Number> GetNormalizedVector(IEnumerable<Number> parameters)
    {
      List<Number> result = new List<Number>(parameters ?? LinqArray<Number>.Empty);
      while (result.Count > 0 && result[^1] == 0)
      {
        result.RemoveAt(result.Count - 1);
      }
      return result.ToArray();
    }

    internal static NumberVector Create(IEnumerable<Number> parameters)
    {
      return new NumberVector(GetNormalizedVector(parameters));
    }

    public NumberVector(params Number[] parameters)
      : this(GetNormalizedVector(parameters))
    {
      // intentionally empty
    }

    private NumberVector(ReadOnlyArray<Number> parameters)
    {
      _values = parameters;
    }

    public static implicit operator NumberVector(in Number v) => new NumberVector(v);
    public static implicit operator NumberVector(in double v) => new NumberVector(v);

    //
    // Properties
    //            

    public Number GetOrDefault(ushort pos)
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
        return _values.AsList();
      }
    }

    public ref readonly Number this[ushort pos]
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

    public ushort Length => Math.Max((ushort)1, _values.Length);

    public Number First => GetInternalEnumerable().First();

    public IEnumerator<Number> GetEnumerator() => GetInternalEnumerable().GetEnumerator();

    

    //
    // IEquatable
    //

    public bool Equals(NumberVector other) => _values.AsList().SequenceEqual(other._values.AsList());

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

    public override bool Equals(object? obj)
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

    public ReadOnlyArray<Number> ToArray(uint length)
    {
      if (length < _values.Length)
      {
        throw new ArgumentException(nameof(length));
      }

      uint diff = length - _values.Length;
      if (diff == 0)
      {
        return _values;
      }
      else
      {
        return _values.AsList().Append(0, diff).ToArray();
      }
    }

    public override int GetHashCode()
    {
      return _values.GetHashCode();
    }
    public int GetHashCodeValue() => GetHashCode();

    public static bool operator ==(in NumberVector a, in NumberVector b) => a.Equals(b);
    public static bool operator !=(in NumberVector a, in NumberVector b) => !a.Equals(b);

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

    public NumberVector Transform(Func<Number, Number> transformation)
    {
      return Create(GetInternalEnumerable().Select(transformation));
    }

    public NumberVector Transform(Func<uint, Number, Number> transformation)
    {
      return Create(GetInternalEnumerable().Select(transformation));
    }

    //
    // Arithmetic operators
    //

    public static NumberVector operator +(NumberVector a, NumberVector b)
    {
      return a.GetInternalEnumerable().ZipValue(b.GetInternalEnumerable(), (va, vb) => (va ?? 0) + (vb ?? 0)).ToVector();
    }

    public static NumberVector operator -(NumberVector a, NumberVector b)
    {
      return a.GetInternalEnumerable().ZipValue(b.GetInternalEnumerable(), (va, vb) => (va ?? 0) - (vb ?? 0)).ToVector();
    }

    public static NumberVector operator /(NumberVector a, Number b)
    {
      return new NumberVector(a.GetInternalEnumerable().Select(v => v / b).ToArray());
    }

    public static NumberVector operator *(NumberVector a, Number b)
    {
      if (b == 0)
      {
        return default;
      }
      else
      {
        return new NumberVector(a.GetInternalEnumerable().Select(v => v * b).ToArray());
      }
    }

    public static NumberVector operator *(in Number a, NumberVector b) => b * a;
    
    //
    // IEnumerable extensions (to avoid boxing)
    //

    public bool AllWithDefault(in Func<Number, bool> predicate)
    {
      return GetInternalEnumerable().AllWithDefault(predicate);
    }
    
    public IEnumerable<TResult> Select<TResult>(Func<Number, TResult> selector)
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

    public IEnumerable<Number> Where(Func<Number, bool> predicate)
    {
      return GetInternalEnumerable().Where(predicate);
    }
  }
}
