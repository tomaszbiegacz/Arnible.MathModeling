namespace Arnible.MathModeling.Analysis.Optimization.SingleStep
{
  public interface ISingleStepOptimization
  {
    /// <summary>
    /// find optimum between startPoint (inclusive) and borderX (exclusive)
    /// </summary>
    Number Optimize(
      in FunctionValueAnalysisForDirection functionToAnalyse,
      in NumberFunctionPointWithDerivative startPoint,
      in Number borderX);
  }
}