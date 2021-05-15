namespace Arnible.MathModeling.Analysis.Optimization
{
  public interface INumberFunctionOptimizationForSmoothSearchRange
  {
    /// <summary>
    /// Improve solution
    /// </summary>
    bool MoveNext(
      in FunctionValueAnalysisForDirection f,
      ref NumberFunctionPointWithDerivative a,
      in Number b);
  }
}