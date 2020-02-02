using System;

namespace Arnible.MathModeling
{
  public class Derivative2Lazy : Derivative1Value, IDerivative2
  {
    private readonly Lazy<Number> _second;

    public Derivative2Lazy(Number first, Func<Number> second)
      : base(first)
    {
      _second = new Lazy<Number>(second);
    }

    public Number Second => _second.Value;

    public override string ToString()
    {
      return $"[{First}, {Second}]";
    }
  }
}
