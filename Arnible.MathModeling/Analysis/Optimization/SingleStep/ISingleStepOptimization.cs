namespace Arnible.MathModeling.Analysis.Optimization.SingleStep
{
  public interface ISingleStepOptimization
  {
    /// <summary>
    /// find optimum between startPoint (inclusive) and maxValue (exclusive)
    /// </summary>
    Number Optimize(
      INumberFunctionWithDerivative f,
      in NumberFunctionPointWithDerivative startPoint,
      in Number maxX);
  }
}