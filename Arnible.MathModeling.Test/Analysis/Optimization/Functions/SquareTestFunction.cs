using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization.Test.Functions
{
  /// <summary>
  /// (x-1)^2 + 3
  /// </summary>
  public record SquareTestFunction : OptimizationTestFunction
  {
    public override ValueWithDerivative1 GetValueWithDerivativeByArgumentsChangeDirection(
      in ReadOnlySpan<Number> arguments,
      in ReadOnlySpan<Number> directionDerivativeRatios)
    {
      arguments.Length.AssertIsEqualTo(1);
      directionDerivativeRatios.Length.AssertIsEqualTo(1);
      
      Number x = arguments[0];
      return new ValueWithDerivative1
      {
        Value = (x - 1).ToPower(2) + 3,
        First = 2*(x-1) * directionDerivativeRatios[0] 
      };
    }
  }
}