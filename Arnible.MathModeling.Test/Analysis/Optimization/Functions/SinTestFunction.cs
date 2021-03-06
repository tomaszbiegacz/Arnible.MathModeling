using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization.Test.Functions
{
  /// <summary>
  /// Sin(x) + 3
  /// </summary>
  public record SinTestFunction : OptimizationTestFunction
  {
    public override ValueWithDerivative1 GetValueWithDerivativeByArgumentsChangeDirection(
      in ReadOnlySpan<Number> arguments,
      in ReadOnlySpan<Number> directionDerivativeRatios)
    {
      arguments.Length.AssertIsEqualTo(1);
      directionDerivativeRatios.Length.AssertIsEqualTo(1);
      
      double x = (double)arguments[0];
      return new ValueWithDerivative1
      {
        Value = Math.Sin(x) + 3,
        First = Math.Cos(x) * directionDerivativeRatios[0] 
      };
    }
  }
}