using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization.Test
{
  /// <summary>
  /// Sin(x) + Cos(y)
  /// </summary>
  /// <remarks>
  /// gnuplot
  /// set xlabel "X"; set ylabel "Y" 
  /// splot [-6:6] [-6:6] sin(x) + cos(y) with pm3d
  /// </remarks>
  public class SinCos2DTestFunction : IFunctionValueAnalysis
  {
    public ValueWithDerivative1 GetValueWithDerivativeByArgumentsChangeDirection(
      in ReadOnlySpan<Number> arguments,
      in ReadOnlySpan<Number> directionDerivativeRatios)
    {
      arguments.Length.AssertIsEqualTo(2);
      directionDerivativeRatios.Length.AssertIsEqualTo(2);
      
      double x = (double)arguments[0];
      double y = (double)arguments[1];
      return new ValueWithDerivative1
      {
        Value = Math.Sin(x) + Math.Cos(y),
        First = Math.Cos(x) * directionDerivativeRatios[0] - Math.Sin(y) * directionDerivativeRatios[1] 
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
      direction.Clear();
      return GetValueWithDerivativeByArgumentsChangeDirection(in arguments, direction).Value;
    }
  }
}