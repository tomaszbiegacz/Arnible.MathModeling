using System;
using Arnible.MathModeling.Algebra;

namespace Arnible.MathModeling
{
  public readonly struct ValueWithDerivative1 : IDerivative1, IValueObject
  {
    public ValueWithDerivative1(in Number value, in Number first)
    {
      Value = value;
      First = first;
    }

    public static explicit operator Derivative1Value(in ValueWithDerivative1 v) => new Derivative1Value(v.First);

    public override string ToString()
    {
      return $"[{First.ToString()}]";
    }
    public string ToStringValue() => ToString();

    public override int GetHashCode()
    {
      return First.GetHashCode();
    }
    public int GetHashCodeValue() => GetHashCode();

    //
    // Properties
    //

    public Number Value { get; }

    public Number First { get; }

    public ValueWithDerivative1 ChangeDirection(in Sign sign)
    {
      switch (sign)
      {
        case Sign.Negative:
          return new ValueWithDerivative1(value: Value, first: -1 * First);
        case Sign.None:
          return new ValueWithDerivative1(value: Value, first: 0);
        case Sign.Positive:
          return this;
      }

      throw new ArgumentException(nameof(sign));
    }
    
    public ValueWithDerivative1 ChangeDirection(in Number direction)
    {
      if (direction == 1)
      {
        return this;
      }
      else
      {
        return new ValueWithDerivative1(value: Value, first: direction * First);
      }
    }
  }
}