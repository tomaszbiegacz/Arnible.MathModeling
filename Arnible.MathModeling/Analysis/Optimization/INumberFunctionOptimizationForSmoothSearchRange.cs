namespace Arnible.MathModeling.Analysis.Optimization
{
  public interface INumberFunctionOptimizationForSmoothSearchRange
  {
    /// <summary>
    /// Improve solution, also considering boundaries
    /// </summary>
    void MoveNext(
      in FunctionValueAnalysisForDirection f,
      ref NumberFunctionOptimizationSearchRange searchRange);
    
    /// <summary>
    /// Improve solution within [a, b) range. 
    /// </summary>
    /// <returns>True if "a" has been improved</returns>
    bool MoveNext(
      in FunctionValueAnalysisForDirection f,
      ref NumberFunctionPointWithDerivative a,
      in Number b,
      out NumberFunctionOptimizationSearchRange searchRange);
  }
}