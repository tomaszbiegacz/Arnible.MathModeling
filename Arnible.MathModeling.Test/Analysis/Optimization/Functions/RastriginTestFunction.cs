using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization.Test.Functions
{
  public record RastriginTestFunction : OptimizationTestFunction
  {
    public Number A { get; init; } = 10;
    public Number B { get; init; } = 2 * Math.PI;
    
    public override ValueWithDerivative1 GetValueWithDerivativeByArgumentsChangeDirection(
      in ReadOnlySpan<Number> arguments,
      in ReadOnlySpan<Number> directionDerivativeRatios)
    {
      arguments.Length.AssertIsEqualTo(directionDerivativeRatios.Length);
      
      Number sumXi2 = 0;
      Number sumXiAi = 0;
      Number sumAiSinBXi = 0;
      Number sumCosBXi = 0;
      for(ushort i=0; i<arguments.Length; ++i)
      {
        ref readonly Number Xi = ref arguments[i]; 
        ref readonly Number Ai = ref directionDerivativeRatios[i];
        Number BXi = B * Xi;
        
        sumXi2 += Xi*Xi;
        sumXiAi += Xi*Ai;
        sumAiSinBXi += Ai*NumberMath.Sin(BXi);
        sumCosBXi += NumberMath.Cos(BXi);
      }
      
      return new ValueWithDerivative1
      {
        Value = A * arguments.Length + sumXi2 - A * sumCosBXi,
        First = 2 * sumXiAi + A * B * sumAiSinBXi
      };
    }
  }
}