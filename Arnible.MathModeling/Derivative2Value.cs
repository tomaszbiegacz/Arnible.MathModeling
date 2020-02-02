using System;

namespace Arnible.MathModeling
{
  public class Derivative2Value : Derivative1Value, IDerivative2
  {
    public Derivative2Value(Number first, Number second)
      : base(first)
    {
      Second = second;
    }

    public Number Second { get; }

    public override string ToString()
    {
      return $"[{First}, {Second}]";
    }
  }
}
