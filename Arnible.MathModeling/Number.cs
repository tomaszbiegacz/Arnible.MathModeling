using System;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public struct Number : IEquatable<Number>
  {
    private readonly double _value;

    private Number(double value)
    {
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

    public override string ToString()
    {
      return _value.ToString();
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

    //
    // Number
    //

    public bool IsValidNumeric => _value.IsValidNumeric();

    public Number ToPower(uint b) => DoubleExtension.ToPower(_value, b);

    public IEnumerable<Number> Yield()
    {
      yield return this;
    }    
  }
}
