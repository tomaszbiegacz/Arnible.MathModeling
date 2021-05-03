namespace Arnible.MathModeling.Analysis.Optimization.SingleStep
{
  public static class ISingleStepOptimizationExtensions
  {
    public static Number Optimize(
      this ISingleStepOptimization optimization,
      INumberFunctionWithDerivative f,
      in Number startPoint,
      in Number maxX)
    {
      return optimization.Optimize(
        f, 
        f.ValueWithDerivative(in startPoint), 
        in maxX);
    }
  }
}