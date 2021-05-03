using System;

namespace Arnible.MathModeling.Analysis.Optimization.Test
{
  /// <summary>
  /// Sin(x) + 3
  /// </summary>
  public class SinTestFunction : INumberFunctionWithDerivative
  {
    private static readonly Number _minValue = -100;
    private static readonly Number _maxValue = 100;
      
    public ref readonly Number MinX => ref _minValue;
    public ref readonly Number MaxX => ref _maxValue;

    public NumberFunctionPointWithDerivative ValueWithDerivative(in Number x)
    {
      return new NumberFunctionPointWithDerivative(
        x: x,
        y: Math.Sin((double)x) + 3,
        first: Math.Cos((double)x)
      );
    }
  }
}