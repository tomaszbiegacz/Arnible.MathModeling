using System;

namespace Arnible.MathModeling.Analysis.Optimization.SingleStep
{
  public static class ISingleStepOptimizationExtensions
  {
    public static Number Optimize(
      this ISingleStepOptimization optimization,
      INumberFunctionWithDerivative f,
      in Number minValue,
      in Number maxValue)
    {
      if(minValue < f.MinValue)
      {
        throw new ArgumentException(nameof(minValue));
      }
      if(maxValue > f.MaxValue)
      {
        throw new ArgumentException(nameof(maxValue));
      }
      
      FunctionPointWithDerivative startPoint = f.ValueWithDerivative(minValue);
      return optimization.Optimize(f, in startPoint, in maxValue);
    }
  }
}