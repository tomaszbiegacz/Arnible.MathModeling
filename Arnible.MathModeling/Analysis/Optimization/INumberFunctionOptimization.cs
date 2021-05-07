namespace Arnible.MathModeling.Analysis.Optimization
{
  public interface INumberFunctionOptimization
  {
    /// <summary>
    /// Improve solution
    /// </summary>
    void MoveNext(ref NumberFunctionOptimizationSearchRange point);
  }
}