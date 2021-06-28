using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization.Test.Functions
{
  /// <summary>
  /// Sin(x) + Cos(y)
  /// </summary>
  /// <remarks>
  /// gnuplot
  /// set xlabel "X"; set ylabel "Y" 
  /// splot [-6:6] [-6:6] sin(x) + cos(y) with pm3d
  /// </remarks>
  public record SinCos2DTestFunction : OptimizationTestFunction
  {
    public override ValueWithDerivative1 GetValueWithDerivativeByArgumentsChangeDirection(
      in ReadOnlySpan<Number> arguments,
      in ReadOnlySpan<Number> directionDerivativeRatios)
    {
      arguments.Length.AssertIsEqualTo(2);
      directionDerivativeRatios.Length.AssertIsEqualTo(2);
      
      double x = (double)arguments[0];
      double y = (double)arguments[1];
      return new ValueWithDerivative1
      {
        Value = Math.Sin(x) + Math.Cos(y),
        First = Math.Cos(x) * directionDerivativeRatios[0] - Math.Sin(y) * directionDerivativeRatios[1] 
      };
    }
  }
}