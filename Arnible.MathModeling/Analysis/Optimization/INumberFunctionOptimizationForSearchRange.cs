namespace Arnible.MathModeling.Analysis.Optimization
{
  public interface INumberFunctionOptimizationForSearchRange
  {
    /// <summary>
    /// Improve solution
    /// </summary>
    void MoveNext(
      in FunctionValueAnalysisForDirection functionToAnalyse,
      ref NumberFunctionOptimizationSearchRange point);
  }
}