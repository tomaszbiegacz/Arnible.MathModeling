using System;

namespace Arnible.MathModeling.Optimization.Test
{
  /// <summary>
  /// Sin(x) + 3
  /// </summary>
  public class SinTestFunction : INumberFunctionWithDerivative
  {
    public Number MinValue => -100;
    public Number MaxValue => 100;

    public FunctionPointWithDerivative ValueWithDerivative(in Number x)
    {
      return new FunctionPointWithDerivative(
        x: x,
        y: Math.Sin((double)x) + 3,
        first: Math.Cos((double)x)
      );
    }
  }
}