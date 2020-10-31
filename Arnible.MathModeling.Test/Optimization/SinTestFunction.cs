using System;

namespace Arnible.MathModeling.Optimization.Test
{
  /// <summary>
  /// Sin(x) + 3
  /// </summary>
  public class SinTestFunction : INumberFunctionWithDerivative
  {
    public ValueWithDerivative1 ValueWithDerivative(in Number x)
    {
      return new ValueWithDerivative1(
        x: x,
        y: Math.Sin((double)x) + 3,
        first: Math.Cos((double)x)
      );
    }
  }
}