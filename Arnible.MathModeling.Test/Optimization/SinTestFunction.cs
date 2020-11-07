using System;

namespace Arnible.MathModeling.Optimization.Test
{
  /// <summary>
  /// Sin(x) + 3
  /// </summary>
  public class SinTestFunction : INumberFunctionWithDerivative
  {
    public NumberValueWithDerivative1 ValueWithDerivative(in Number x)
    {
      return new NumberValueWithDerivative1(
        x: x,
        y: Math.Sin((double)x) + 3,
        first: Math.Cos((double)x)
      );
    }
  }
}