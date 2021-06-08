using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization.Test
{
  /// <summary>
  /// Sin(x) + 3
  /// </summary>
  public record SinTestFunction : IFunctionValueAnalysis
  {
    public ValueWithDerivative1 GetValueWithDerivativeByArgumentsChangeDirection(
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
    
    public void GradientByArguments(in ReadOnlySpan<Number> arguments, in Span<Number> result)
    {
      result.Length.AssertIsEqualTo(1);
      Span<Number> direction = stackalloc Number[1];
      
      direction[0] = 1;
      result[0] = GetValueWithDerivativeByArgumentsChangeDirection(in arguments, direction).First;
    }
    
    public Number GetValue(in ReadOnlySpan<Number> arguments)
    {
      Span<Number> direction = stackalloc Number[1];
      direction[0] = 1;
      return GetValueWithDerivativeByArgumentsChangeDirection(in arguments, direction).Value;
    }
  }
}