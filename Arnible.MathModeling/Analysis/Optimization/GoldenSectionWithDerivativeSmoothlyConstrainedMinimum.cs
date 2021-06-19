using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization
{
  /// <summary>
  /// Golden section search for minimum with exclusive border.
  /// Search is capable of dealing with multimodal functions and it uses sign of derivative 
  /// to find section where secant method can be used to speed up optimization task.    
  /// </summary>
  public record GoldenSectionWithDerivativeSmoothlyConstrainedMinimum : INumberFunctionOptimizationForSmoothSearchRange
  {
    const double Ratio = GoldenSectionWithDerivativeConstrainedMinimum.Ratio;
    
    private readonly GoldenSectionWithDerivativeConstrainedMinimum _goldenSection;

    public GoldenSectionWithDerivativeSmoothlyConstrainedMinimum(ISimpleLogger logger)
    {
      _goldenSection = new GoldenSectionWithDerivativeConstrainedMinimum(logger);
    }
    
    public void MoveNext(
      in FunctionValueAnalysisForDirection f,
      ref NumberFunctionOptimizationSearchRange searchRange)
    {
      _goldenSection.MoveNext(in f, ref searchRange);
    }
    
    public bool MoveNext(
      in FunctionValueAnalysisForDirection f,
      ref NumberFunctionPointWithDerivative a,
      in Number b,
      out NumberFunctionOptimizationSearchRange searchRange)
    {
      Number width = b - a.X;
      width.AssertIsNotEqualTo(0);
      Number x1 = b - Ratio * width;
      Number x2 = a.X + Ratio * width;
      
      searchRange = new(in a, f.ValueWithDerivative(in x2));
      if (!searchRange.IsEmptyRange)
      {
        _goldenSection.MoveNext(ref searchRange, f.ValueWithDerivative(in x1));  
      }
      
      if (searchRange.BorderSmaller.X == a.X)
      {
        return false;
      }
      else
      {
        a = searchRange.BorderSmaller;
        return true;
      }
    }
  }
}