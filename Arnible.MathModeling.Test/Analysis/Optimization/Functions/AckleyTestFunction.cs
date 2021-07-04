using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization.Test.Functions
{
  /// <summary>
  /// https://www.sfu.ca/~ssurjano/ackley.html
  /// </summary>
  /// <remarks>
  /// Derivative does not exists at optimum due to sqrt function
  /// </remarks>
  public record AckleyTestFunction : OptimizationTestFunction
  {
    public Number A { get; init; } = 20;
    public Number B { get; init; } = 0.2;
    public Number C { get; init; } = 2 * Math.PI;
    
    public override ValueWithDerivative1 GetValueWithDerivativeByArgumentsChangeDirection(
      in ReadOnlySpan<Number> arguments,
      in ReadOnlySpan<Number> directionDerivativeRatios)
    {
      arguments.Length.AssertIsEqualTo(directionDerivativeRatios.Length);
      
      Number sumXi2 = 0;
      Number sumXiAi = 0;
      Number sumAiSinCXi = 0;
      Number sumCosCXi = 0;
      for(ushort i=0; i<arguments.Length; ++i)
      {
        ref readonly Number Xi = ref arguments[i]; 
        ref readonly Number Ai = ref directionDerivativeRatios[i];
        Number CXi = C * Xi;
        
        sumXi2 += Xi*Xi;
        sumXiAi += Xi*Ai;
        sumAiSinCXi += Ai*NumberMath.Sin(CXi);
        sumCosCXi += NumberMath.Cos(CXi);
      }
      
      Number d = arguments.Length;
      Number sqrtD = NumberMath.Sqrt(d);
      Number sqrtSumXi2 = NumberMath.Sqrt(sumXi2);
      
      Number e1 = NumberMath.Exp(-1 * B / sqrtD * sqrtSumXi2);
      Number e2 = NumberMath.Exp(sumCosCXi / d);
      
      Number sumXiAi_sqrtSumXi2;
      if(sqrtSumXi2 == 0)
      {
        sumXiAi.AssertIsEqualTo(0);
        // this is not true in general, value depends on limes direction
        sumXiAi_sqrtSumXi2 = 1;
      }
      else
      {
        sumXiAi_sqrtSumXi2 = sumXiAi/sqrtSumXi2;
      }
      return new ValueWithDerivative1
      {
        Value = A + Math.E - A * e1 - e2,
        First = A*B/sqrtD*sumXiAi_sqrtSumXi2*e1 + C/d*sumAiSinCXi*e2
      };
    }
  }
}