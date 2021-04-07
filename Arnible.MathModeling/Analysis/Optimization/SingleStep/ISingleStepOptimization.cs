namespace Arnible.MathModeling.Analysis.Optimization.SingleStep
{
  public interface ISingleStepOptimization
  {
    Number Optimize(
      INumberFunctionWithDerivative f,
      in FunctionPointWithDerivative a,
      in Number b);
  }
}