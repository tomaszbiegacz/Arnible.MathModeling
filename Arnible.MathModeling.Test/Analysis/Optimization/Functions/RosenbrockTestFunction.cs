using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization.Test.Functions
{
  /// <summary>
  /// https://en.wikipedia.org/wiki/Rosenbrock_function
  /// </summary>
  /// <remarks>
  /// gnuplot
  /// set xlabel "X"; set ylabel "Y" 
  /// splot [-3:3] [-3:3] (1-x)**2 + 100*(y - x**2)**2 with pm3d
  /// </remarks> 
  public record RosenbrockTestFunction : OptimizationTestFunction
  {
    public Number A { get; init; } = 1;
    public Number B { get; init; } = 100;
    
    public override ValueWithDerivative1 GetValueWithDerivativeByArgumentsChangeDirection(
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
  }
}