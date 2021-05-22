using System;

namespace Arnible.MathModeling.Analysis.Optimization
{
  public interface IFunctionValueAnalysis
  {
    /// <summary>
    /// Value and it's derivative by arguments changing in given direction
    /// </summary>
    /// <param name="arguments">Position where to calculate value and derivative</param>
    /// <param name="directionDerivativeRatios">Ration in hyperspherical coordinates</param>
    ValueWithDerivative1 ValueWithDerivativeByArgumentsChangeDirection(
      in ReadOnlySpan<Number> arguments,
      in ReadOnlySpan<Number> directionDerivativeRatios);
  }
}