namespace Arnible.MathModeling.Analysis.Optimization
{
  public class GoldenSecantMinimum : INumberFunctionOptimization
  {
    private readonly GoldenSectionWithDerivativeConstrainedMinimum _goldenSection;
    private readonly UnimodalSecantMinimum _secant;

    public GoldenSecantMinimum(ISimpleLogger logger)
    {
      _goldenSection = new GoldenSectionWithDerivativeConstrainedMinimum(logger);
      _secant = new UnimodalSecantMinimum(logger);
    }

    public void MoveNext(ref NumberFunctionOptimizationSearchRange point)
    {
      UnimodalSecantAnalysis secantApplication = point.GetSecantApplicability();
      if(secantApplication == UnimodalSecantAnalysis.HasMinimum)
      {
        try
        {
          _secant.MoveNext(ref point);
        }
        catch(MultimodalFunctionException)
        {
          // fallback
          _goldenSection.MoveNext(ref point);
        }  
      }
      else
      {
        _goldenSection.MoveNext(ref point);
      }
    }
  }
}