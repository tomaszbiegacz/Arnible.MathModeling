using System;

namespace Arnible.MathModeling.Analysis.Optimization
{
  public interface IFunctionValueAnalysis
  {
    /// <summary>
    /// Calculate function's value
    /// </summary>
    Number GetValue(in ReadOnlySpan<Number> arguments);
    
    /// <summary>
    /// Value and it's derivative by arguments changing in given direction
    /// </summary>
    /// <param name="arguments">Position where to calculate value and derivative</param>
    /// <param name="directionDerivativeRatios">Ration in hyperspherical coordinates</param>
    ValueWithDerivative1 GetValueWithDerivativeByArgumentsChangeDirection(
      in ReadOnlySpan<Number> arguments,
      in ReadOnlySpan<Number> directionDerivativeRatios);
  }
}