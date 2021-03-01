using System;

namespace Arnible.MathModeling.Optimization.Test
{
  /// <summary>
  /// Sin(x) + 3
  /// </summary>
  public class SinTestFunction : INumberFunctionWithDerivative
  {
    private static readonly Number _minValue = -100;
    private static readonly Number _maxValue = 100;
      
    public ref readonly Number MinValue => ref _minValue;
    public ref readonly Number MaxValue => ref _maxValue;

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