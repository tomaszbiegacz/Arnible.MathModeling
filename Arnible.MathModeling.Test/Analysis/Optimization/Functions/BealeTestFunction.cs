using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization.Test.Functions
{
  public record BealeTestFunction : OptimizationTestFunction
  {
    public override ValueWithDerivative1 GetValueWithDerivativeByArgumentsChangeDirection(
      in ReadOnlySpan<Number> arguments,
      in ReadOnlySpan<Number> directionDerivativeRatios)
    {
      arguments.Length.AssertIsEqualTo(2);
      arguments.Length.AssertIsEqualTo(directionDerivativeRatios.Length);
      
      ref readonly Number x = ref arguments[0];
      ref readonly Number y = ref arguments[1];
      
      ref readonly Number xd = ref directionDerivativeRatios[0];
      ref readonly Number yd = ref directionDerivativeRatios[1];
      
      Number y2 = y.ToPower(2);
      Number y3 = y.ToPower(3);
      
      Number c1 = 1.5 - x + x*y;
      Number c2 = 2.25 - x + x*y2;
      Number c3 = 2.625 - x + x*y3;
      
      return new ValueWithDerivative1
      {
        Value = c1.ToPower(2) + c2.ToPower(2) + c3.ToPower(2),
        First = 2*c1*(-1*xd + xd*y + x*yd) + 2*c2*(-1*xd + xd*y2 + x*2*y*yd) + 2*c3*(-1*xd + xd*y3 + x*3*y2*yd)
      };
    }
  }
}