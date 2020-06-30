using Arnible.MathModeling.Export;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Arnible.MathModeling
{
  [Serializable]
  [RecordSerializer(SerializationMediaType.TabSeparatedValues, typeof(Serializer))]
  public readonly struct NumberArray : IEquatable<NumberArray>, IArray<Number>
  {
    class Serializer : ToStringSerializer<NumberArray>
    {
      public Serializer() : base(v => v.ToString(CultureInfo.InvariantCulture))
      {
        // intentionally empty
      }
    }

    public static NumberArray Repeat(Number value, uint length)
    {
      return new NumberArray(LinqEnumerable.Repeat(value, length).ToValueArray());
    }

    private readonly ValueArray<Number> _values;

    internal static NumberArray Create(IEnumerable<Number> src)
    {
      return new NumberArray(src.ToValueArray());
    }

    public NumberArray(params Number[] parameters)
      : this(parameters.ToValueArray())
    {
      // intentionally empty
    }

    private NumberArray(ValueArray<Number> parameters)
    {
      _values = parameters;
    }

    //
    // Properties
    //                

    public bool IsZero => _values.All(v => v == 0);

    //
    // IArray
    //    

    public Number this[uint pos] => _values[pos];

    public uint Length => _values.Length;

    public IEnumerator<Number> GetEnumerator() => _values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

    //
    // IEquatable
    //

    public bool Equals(NumberArray other) => _values.SequenceEqual(other._values);


    public override bool Equals(object obj)
    {
      if (obj is NumberArray v)
      {
        return Equals(v);
      }
      else
      {
        return false;
      }
    }

    public override string ToString() => ToString(CultureInfo.InvariantCulture);

    public string ToString(CultureInfo cultureInfo)
    {
      return "[" + string.Join(" ", _values.Select(v => v.ToString(cultureInfo))) + "]";
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

    public static bool operator ==(NumberArray a, NumberArray b) => a.Equals(b);
    public static bool operator !=(NumberArray a, NumberArray b) => !a.Equals(b);

    //
    // query operators
    //

    public NumberArray Transform(Func<Number, Number> transformation)
    {
      return new NumberArray(_values.Select(transformation).ToValueArray());
    }

    public NumberArray Transform(Func<uint, Number, Number> transformation)
    {
      return new NumberArray(_values.Select(transformation).ToValueArray());
    }
  }
}
