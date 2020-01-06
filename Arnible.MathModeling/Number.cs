using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public struct Number
  {
    private readonly double _value;

    private Number(double value)
    {
      _value = value;
    }

    public static implicit operator Number(double v) => new Number(v);
    public static implicit operator double(Number v) => v._value;

    public bool IsValidNumeric() => _value.IsValidNumeric();

    public Number ToPower(uint b) => DoubleExtension.ToPower(_value, b);

    public IEnumerable<Number> Yield()
    {
      yield return this;
    }
  }
}
