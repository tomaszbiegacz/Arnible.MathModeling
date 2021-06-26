using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization.Test.Functions
{
  /// <summary>
  /// https://en.wikipedia.org/wiki/Rosenbrock_function
  /// </summary>
  public record RosenbrockGeneralisationTestFunction : OptimizationTestFunction
  {
    private readonly RosenbrockTestFunction _rosenbrock = new();

    public override ValueWithDerivative1 GetValueWithDerivativeByArgumentsChangeDirection(
      in ReadOnlySpan<Number> arguments,
      in ReadOnlySpan<Number> directionDerivativeRatios)
    {
      arguments.Length.AssertIsGreaterThan(1);
      
      Number value = 0;
      Number firstDerivative = 0;
      for(ushort i=0; i<arguments.Length-1; i++)
      {
        var result = _rosenbrock.GetValueWithDerivativeByArgumentsChangeDirection(
          arguments: arguments[i..(i+2)],
          directionDerivativeRatios: directionDerivativeRatios[i..(i+2)]); 
        value += result.Value;
        firstDerivative += result.First;
      }
      return new ValueWithDerivative1
      {
        Value = value,
        First = firstDerivative
      };
    }
  }
}