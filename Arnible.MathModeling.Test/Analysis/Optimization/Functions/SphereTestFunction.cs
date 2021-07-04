using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization.Test.Functions
{
  public record SphereTestFunction : OptimizationTestFunction
  {
    public override ValueWithDerivative1 GetValueWithDerivativeByArgumentsChangeDirection(
      in ReadOnlySpan<Number> arguments,
      in ReadOnlySpan<Number> directionDerivativeRatios)
    {
      arguments.Length.AssertIsEqualTo(directionDerivativeRatios.Length);
      
      Number sumXi2 = 0;
      Number sumXiAi = 0;
      for(ushort i=0; i<arguments.Length; ++i)
      {
        ref readonly Number Xi = ref arguments[i]; 
        ref readonly Number Ai = ref directionDerivativeRatios[i];

        sumXi2 += Xi*Xi;
        sumXiAi += Xi*Ai;
      }
      
      return new ValueWithDerivative1
      {
        Value = sumXi2,
        First = 2 * sumXiAi
      };
    }
  }
}