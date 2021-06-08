using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization.Test
{
  /// <summary>
  /// https://en.wikipedia.org/wiki/Rosenbrock_function
  /// </summary>
  public record RosenbrockTestFunction : IFunctionValueAnalysis
  {
    public Number A { get; init; } = 1;
    public Number B { get; init; } = 100;
    
    public ValueWithDerivative1 GetValueWithDerivativeByArgumentsChangeDirection(
      in ReadOnlySpan<Number> arguments,
      in ReadOnlySpan<Number> directionDerivativeRatios)
    {
      arguments.Length.AssertIsEqualTo(2);
      directionDerivativeRatios.Length.AssertIsEqualTo(2);
      
      ref readonly Number x = ref arguments[0];
      ref readonly Number dx = ref directionDerivativeRatios[0];
      
      ref readonly Number y = ref arguments[1];
      ref readonly Number dy = ref directionDerivativeRatios[1];
      return new ValueWithDerivative1
      {
        Value = (A - x).ToPower(2) + B * (y - x.ToPower(2)).ToPower(2),
        First = 
          2*(A - x)*(-1)*dx + B*2*(y - x.ToPower(2))*(dy - 2*x*dx) 
      };
    }

    public void GradientByArguments(in ReadOnlySpan<Number> arguments, in Span<Number> result)
    {
      result.Length.AssertIsEqualTo(2);
      Span<Number> direction = stackalloc Number[2];
      
      direction[0] = 1;
      direction[1] = 0;
      result[0] = GetValueWithDerivativeByArgumentsChangeDirection(in arguments, direction).First;
      
      direction[0] = 0;
      direction[1] = 1;
      result[1] = GetValueWithDerivativeByArgumentsChangeDirection(in arguments, direction).First;
    }

    public Number GetValue(in ReadOnlySpan<Number> arguments)
    {
      Span<Number> direction = stackalloc Number[2];
      direction[0] = 1;
      direction[1] = 1;
      return GetValueWithDerivativeByArgumentsChangeDirection(in arguments, direction).Value;
    }
  }
}