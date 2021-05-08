namespace Arnible.MathModeling.Analysis.Optimization
{
  public interface INumberFunctionOptimization
  {
    /// <summary>
    /// Improve solution
    /// </summary>
    void MoveNext(
      in FunctionValueAnalysisForDirection functionToAnalyse,
      ref NumberFunctionOptimizationSearchRange point);
  }
}