using Arnible.MathModeling.Export;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Arnible.MathModeling.Algebra
{
  [Serializable]
  [RecordSerializer(SerializationMediaType.TabSeparatedValues, typeof(Serializer))]
  public readonly struct NumberVector : IEquatable<NumberVector>, IEquatable<Number>, IArray<Number>
  {
    private readonly static IEnumerable<Number> Zero = new Number[] { 0 }.ToValueArray();

    class Serializer : ToStringSerializer<NumberVector>
    {
      public Serializer() : base(v => v.ToString(CultureInfo.InvariantCulture))
      {
        // intentionally empty
      }
    }

    public static NumberVector Repeat(Number value, uint length)
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

    public static NumberVector NonZeroValueAt(uint pos, Number value)
    {
      if (value == 0)
      {
        throw new ArgumentException(nameof(value));
      }

      return new NumberVector(LinqEnumerable.Repeat<Number>(0, pos).Append(value).ToValueArray());
    }

    public static NumberVector NonZeroValueAt(IArray<bool> pos, Number value)
    {
      if (value == 0)
      {
        throw new ArgumentException(nameof(value));
      }

      return Create(pos.Select(v => v ? value : 0));
    }

    public static NumberVector NonZeroValueAt(IArray<Sign> pos, Number value)
    {
      if (value == 0)
      {
        throw new ArgumentException(nameof(value));
      }

      return Create(pos.Select(v => v == Sign.None ? 0 : (int)v*value));
    }

    private readonly ValueArray<Number> _values;

    private static ValueArray<Number> GetNormalizedVector(IEnumerable<Number> parameters)
    {
      List<Number> result = new List<Number>(parameters ?? LinqEnumerable.Empty<Number>());
      while (result.Count > 0 && result[result.Count - 1] == 0)
      {
        result.RemoveAt(result.Count - 1);
      }
      return result.ToValueArray();
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

    private NumberVector(ValueArray<Number> parameters)
    {
      _values = parameters;
    }

    public static implicit operator NumberVector(Number v) => new NumberVector(v);
    public static implicit operator NumberVector(double v) => new NumberVector(v);

    //
    // Properties
    //            

    public Number GetOrDefault(uint pos)
    {
      if (pos >= Length)
        return 0;
      else
        return _values[pos];
    }

    //
    // IArray
    //

    private IEnumerable<Number> GetInternalEnumerator()
    {
      if(_values.Length == 0)
      {
        return Zero;
      }
      else
      {
        return _values;
      }
    }

    public Number this[uint pos]
    {
      get
      {
        if(pos == 0 && _values.Length == 0)
        {
          return 0;
        }
        else
        {
          return _values[pos];
        }
      }
    }

    public uint Length => Math.Max(1, _values.Length);

    public IEnumerator<Number> GetEnumerator() => GetInternalEnumerator().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetInternalEnumerator().GetEnumerator();

    //
    // IEquatable
    //

    public bool Equals(NumberVector other) => _values.SequenceEqual(other._values);

    public bool Equals(Number other)
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

    public override bool Equals(object obj)
    {
      if (obj is NumberVector v)
      {
        return Equals(v);
      }
      else if (obj is Number v2)
      {
        return Equals(v2);
      }
      else
      {
        return false;
      }
    }

    public override string ToString() => ToString(CultureInfo.InvariantCulture);

    public string ToString(CultureInfo cultureInfo)
    {
      if (Length == 1)
      {
        return this[0].ToString(cultureInfo);
      }
      else
      {
        return "[" + string.Join(" ", GetInternalEnumerator().Select(v => v.ToString(cultureInfo))) + "]";
      }
    }

    public override int GetHashCode()
    {
      int hc = Length.GetHashCode();
      foreach (var v in _values)
      {
        hc = unchecked(hc * 314159 + v.GetHashCode());
      }
      return hc;
    }

    public static bool operator ==(NumberVector a, NumberVector b) => a.Equals(b);
    public static bool operator !=(NumberVector a, NumberVector b) => !a.Equals(b);

    public static bool operator ==(Number a, NumberVector b) => b.Equals(a);
    public static bool operator !=(Number a, NumberVector b) => !b.Equals(a);

    public static bool operator ==(NumberVector a, Number b) => a.Equals(b);
    public static bool operator !=(NumberVector a, Number b) => !a.Equals(b);

    //
    // query operators
    //

    public NumberVector Transform(Func<Number, Number> transformation)
    {
      return Create(GetInternalEnumerator().Select(transformation));
    }

    public NumberVector Transform(Func<uint, Number, Number> transformation)
    {
      return Create(GetInternalEnumerator().Select(transformation));
    }

    public NumberArray ToArray(uint size)
    {
      if (size < Length)
      {
        throw new ArgumentException(nameof(size), $"requested {size} whereas minimum is {Length}");
      }
      NumberTranslationVector translation = new NumberTranslationVector(this);
      return translation.Translate(NumberArray.Repeat(0, size));
    }

    //
    // Arithmetic operators
    //

    public static NumberVector operator +(NumberVector a, NumberVector b) => a.Zip(b, (va, vb) => (va ?? 0) + (vb ?? 0)).ToVector();
    public static NumberVector operator -(NumberVector a, NumberVector b) => a.Zip(b, (va, vb) => (va ?? 0) - (vb ?? 0)).ToVector();

    public static NumberVector operator /(NumberVector a, Number b) => new NumberVector(a.Select(v => v / b).ToValueArray());

    public static NumberVector operator *(NumberVector a, Number b)
    {
      if (b == 0)
      {
        return default;
      }
      else
      {
        return new NumberVector(a.Select(v => v * b).ToValueArray());
      }
    }

    public static NumberVector operator *(Number a, NumberVector b) => b * a;
  }
}
