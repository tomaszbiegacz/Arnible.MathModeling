namespace Arnible.MathModeling.Analysis.Optimization
{
  public class GoldenSecantMinimum : INumberFunctionOptimizationForSearchRange
  {
    private readonly GoldenSectionWithDerivativeConstrainedMinimum _goldenSection;
    private readonly UnimodalSecantMinimum _secant;

    public GoldenSecantMinimum(ISimpleLogger logger)
    {
      _goldenSection = new GoldenSectionWithDerivativeConstrainedMinimum(logger);
      _secant = new UnimodalSecantMinimum(logger);
    }

    public void MoveNext(
      in FunctionValueAnalysisForDirection functionToAnalyse,
      ref NumberFunctionOptimizationSearchRange point)
    {
      UnimodalSecantAnalysis secantApplication = point.GetSecantApplicability();
      if(secantApplication == UnimodalSecantAnalysis.HasMinimum)
      {
        try
        {
          _secant.MoveNext(in functionToAnalyse, ref point);
        }
        catch(MultimodalFunctionException)
        {
          // fallback
          _goldenSection.MoveNext(in functionToAnalyse, ref point);
        }  
      }
      else
      {
        _goldenSection.MoveNext(in functionToAnalyse, ref point);
      }
    }
  }
}