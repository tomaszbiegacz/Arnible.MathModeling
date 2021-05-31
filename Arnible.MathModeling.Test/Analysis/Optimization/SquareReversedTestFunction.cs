using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization.Test
{
  /// <summary>
  /// 3 - (x-1)^2
  /// </summary>
  public record SquareReversedTestFunction : IFunctionValueAnalysis
  {
    public ValueWithDerivative1 GetValueWithDerivativeByArgumentsChangeDirection(
      in ReadOnlySpan<Number> arguments,
      in ReadOnlySpan<Number> directionDerivativeRatios)
    {
      arguments.Length.AssertIsEqualTo(1);
      directionDerivativeRatios.Length.AssertIsEqualTo(1);
      
      Number x = arguments[0];
      return new ValueWithDerivative1
      {
        Value = 3 - (x - 1).ToPower(2),
        First = -2*(x-1) * directionDerivativeRatios[0] 
      };
    }
    
    public Number GetValue(in ReadOnlySpan<Number> arguments)
    {
      return GetValueWithDerivativeByArgumentsChangeDirection(in arguments, new Number[] { 1 }).Value;
    }
  }
}