using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization.SingleStep
{
  /// <summary>
  /// Combination of golden section and secant optimization methods
  /// for search in direction with negative 1st derivative
  /// </summary>
  public record GoldenSecantSmoothlyConstrainedMinimum : ISingleStepOptimization
  {
    const double Ratio = GoldenSectionWithDerivativeConstrainedMinimum.Ratio;
    
    private readonly ISimpleLogger _logger;
    private readonly GoldenSectionWithDerivativeConstrainedMinimum _goldenSection;
    private readonly UnimodalSecantMinimum _secant;

    public GoldenSecantSmoothlyConstrainedMinimum(ISimpleLogger logger)
    {
      _logger = logger;
      _goldenSection = new(logger);
      _secant = new(logger);
    }
    
    private void ApplyUnimodalSecant(
      in FunctionValueAnalysisForDirection f,
      in NumberFunctionPointWithDerivative a,
      ref NumberFunctionPointWithDerivative b,
      ref bool bImproved,
      ref NumberFunctionPointWithDerivative result)
    {
      NumberFunctionOptimizationSearchRange r = new(in a, in b);
      if(_secant.TryMoveNext(in f, ref r))
      {
        if (r.BorderSmaller.Y < result.Y)
        {
          result = r.BorderSmaller;  
        }
        
        if (r.BorderHighestDerivative.Y < b.Y)
        {
          b = r.BorderHighestDerivative;
          bImproved = true;
        }
      }
    }
    
    private void ApplyGoldenSection(
      in FunctionValueAnalysisForDirection f,
      in NumberFunctionPointWithDerivative a,
      in NumberFunctionPointWithDerivative b,
      ref NumberFunctionPointWithDerivative result)
    {
      NumberFunctionOptimizationSearchRange r = new(in a, in b);
      _goldenSection.MoveNext(in f, ref r);
      if(r.BorderSmaller.Y < result.Y)
      {
        result = r.BorderSmaller;
      }
    }
    
    public void MoveNext(
      in FunctionValueAnalysisForDirection f,
      ref NumberFunctionOptimizationSearchRange searchRange)
    {
      NumberFunctionOptimizationSearchRange secantResult = searchRange;
      if(secantResult.GetSecantApplicability() == UnimodalSecantAnalysis.HasMinimum)
      {
        _logger.Log("> Secant");
        _secant.TryMoveNext(in f, ref secantResult);  
      }
      
      _logger.Log("> Golden section");
      _goldenSection.MoveNext(in f, ref searchRange);
      
      if (secantResult.BorderSmaller.Y < searchRange.BorderSmaller.Y)
      {
        searchRange = secantResult;
      }
    }

    public NumberFunctionPointWithDerivative MoveNext(
      in FunctionValueAnalysisForDirection f,
      in NumberFunctionPointWithDerivative a,
      in Number b)
    {
      a.First.AssertIsLessThan(0);
      
      Number width = b - a.X;
      width.AssertIsGreaterThan(0);
      
      NumberFunctionPointWithDerivative p1 = f.ValueWithDerivative(b - Ratio * width);
      NumberFunctionPointWithDerivative p2 = f.ValueWithDerivative(a.X + Ratio * width);
      NumberFunctionPointWithDerivative result = a;

      bool p1Improved = false;
      if (p1.First > 0)
      {
        _logger.Log("> p1 secant");
        ApplyUnimodalSecant(in f, in a, ref p1, ref p1Improved, ref result);
      }
      else
      {
        _logger.Log("> p1 golden section");
        ApplyGoldenSection(in f, in a, in p1, ref result);
      }
      
      bool p2Improved = false;
      if (p2.First > 0)
      {
        _logger.Log("> p2 secant");
        ApplyUnimodalSecant(in f, in a, ref p2, ref p2Improved, ref result);
      }
      else
      {
        _logger.Log("> p2 golden section");
        ApplyGoldenSection(in f, in a, in p2, ref result);
      }
      
      if (result.Y >= a.Y)
      {
        if (p2Improved)
        {
          _logger.Log(" Narrowing range to [a, p2)");
          return MoveNext(in f, in a, p2.X);
        } 
        else if (p1Improved)
        {
          _logger.Log(" Narrowing range to [a, p1)");
          return MoveNext(in f, in a, p1.X);
        }
        else
        {
          _logger.Log(" Narrowing range to [p2, b)");
          return MoveNext(in f, in p2, in b);  
        }
      }
      else
      {
        return result;
      }
    }
  }
}