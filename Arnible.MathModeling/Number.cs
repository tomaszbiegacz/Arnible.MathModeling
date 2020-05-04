using Arnible.MathModeling.Export;
using System;
using System.Globalization;

namespace Arnible.MathModeling
{
  [Serializable]
  [RecordSerializer(SerializationMediaType.TabSeparatedValues, typeof(Serializer))]
  public readonly struct Number : IEquatable<Number>, IComparable<Number>
  {
    class Serializer : ToStringSerializer<Number>
    {
      public Serializer() : base(v => v.ToString(CultureInfo.InvariantCulture))
      {
        // intentionally empty
      }
    }

    private readonly double _value;

    private Number(double value)
    {
      if (!value.IsValidNumeric())
      {
        throw new ArgumentException($"{nameof(value)}: [{value}]");
      }
      _value = value;
    }

    public static implicit operator Number(double v) => new Number(v);
    public static implicit operator double(Number v) => v._value;

    //
    // Object
    //

    public override bool Equals(object obj)
    {
      if (obj is Number v)
      {
        return Equals(v);
      }
      else
      {
        return false;
      }
    }

    public override int GetHashCode()
    {
      return _value.GetHashCode();
    }

    public override string ToString() => ToString(CultureInfo.InvariantCulture);    

    public string ToString(CultureInfo cultureInfo)
    {
      return _value.ToString(cultureInfo);
    }

    //
    // Operators
    //

    public static bool operator ==(Number a, Number b) => a.Equals(b);
    public static bool operator !=(Number a, Number b) => !a.Equals(b);
    public bool Equals(Number other)
    {
      return _value.NumericEquals(other._value);
    }

    public static Number operator /(Number a, double b) => new Number(a._value / b);

    public static Number operator +(Number a, Number b) => new Number(a._value + b._value);    

    public static Number operator -(Number a, Number b) => new Number(a._value - b._value);    

    public static Number operator *(Number a, Number b) => new Number(a._value * b._value);

    //
    // Number
    //

    public static bool operator <(Number a, Number b) => a._value < b._value;
    public static bool operator >(Number a, Number b) => a._value > b._value;
    public static bool operator <=(Number a, Number b) => a._value < b._value || a.Equals(b);
    public static bool operator >=(Number a, Number b) => a._value > b._value || a.Equals(b);

    public Number ToPower(uint b) => DoubleExtension.ToPower(_value, b);

    public int CompareTo(Number other)
    {
      if (_value == other._value) return 0;
      return _value > other._value ? 1 : -1;
    }
  }
}
