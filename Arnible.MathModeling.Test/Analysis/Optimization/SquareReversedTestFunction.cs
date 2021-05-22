using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization.Test
{
  /// <summary>
  /// 3 - (x-1)^2
  /// </summary>
  public record SquareReversedTestFunction : IFunctionValueAnalysis
  {
    public ValueWithDerivative1 ValueWithDerivativeByArgumentsChangeDirection(
      in ReadOnlySpan<Number> arguments,
      in ReadOnlySpan<Number> directionDerivativeRatios)
    {
      arguments.Length
        .AssertIsEqualTo(1)
        .AssertIsEqualTo(directionDerivativeRatios.Length);
      
      Number x = arguments[0];
      return new ValueWithDerivative1
      {
        Value = 3 - (x - 1).ToPower(2),
        First = -2*(x-1) * directionDerivativeRatios[0] 
      };
    }
  }
}