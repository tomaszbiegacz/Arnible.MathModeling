using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization.Test.Functions
{
  public abstract record OptimizationTestFunction : IFunctionValueAnalysis
  {
    public abstract ValueWithDerivative1 GetValueWithDerivativeByArgumentsChangeDirection(
      in ReadOnlySpan<Number> arguments,
      in ReadOnlySpan<Number> directionDerivativeRatios);
    
    public void GradientByArguments(in ReadOnlySpan<Number> arguments, in Span<Number> result)
    {
      result.Length.AssertIsEqualTo(arguments.Length);
      Span<Number> direction = stackalloc Number[arguments.Length];
      
      for(ushort i=0; i<result.Length; ++i)
      {
        direction.Clear();
        direction[i] = 1;
        result[i] = GetValueWithDerivativeByArgumentsChangeDirection(in arguments, direction).First;  
      }
    }

    public Number GetValue(in ReadOnlySpan<Number> arguments)
    {
      Span<Number> direction = stackalloc Number[arguments.Length];
      return GetValueWithDerivativeByArgumentsChangeDirection(in arguments, direction).Value;
    }
  }
}