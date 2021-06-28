namespace Arnible.MathModeling.Analysis.Optimization.SingleStep
{
  public interface ISingleStepOptimization
  {
    /// <summary>
    /// find optimum between startPoint (inclusive) and borderX (exclusive)
    /// </summary>
    NumberFunctionPointWithDerivative MoveNext(
      in FunctionValueAnalysisForDirection functionToAnalyse,
      in NumberFunctionPointWithDerivative startPoint,
      in Number borderX,
      out uint complexity);
    
    void MoveNext(
      in FunctionValueAnalysisForDirection f,
      ref NumberFunctionOptimizationSearchRange searchRange);
  }
}