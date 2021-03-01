namespace Arnible.MathModeling.Optimization.SingleStep
{
  public static class ISingleStepOptimizationExtensions
  {
    public static Number Optimize(
      this ISingleStepOptimization optimization,
      INumberFunctionWithDerivative f)
    {
      FunctionPointWithDerivative startPoint = f.ValueWithDerivative(f.MinValue);
      return optimization.Optimize(f, in startPoint, in f.MaxValue);
    }
  }
}