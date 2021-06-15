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
        _logger.Write("> Secant").NewLine();
        _secant.TryMoveNext(in f, ref secantResult);  
      }
      
      _logger.Write("> Golden section").NewLine();
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

      // try to improve with p1
      bool p1ImprovedSecant = false;
      if (p1.First > 0)
      {
        _logger.Write("> p1 secant").NewLine();
        ApplyUnimodalSecant(in f, in a, ref p1, ref p1ImprovedSecant, ref result);
      }
      else
      {
        _logger.Write("> p1 golden section").NewLine();
        ApplyGoldenSection(in f, in a, in p1, ref result);
      }
      
      // try to improve with p2
      bool p2ImprovedSecant = false;
      if (p2.First > 0)
      {
        _logger.Write("> p2 secant").NewLine();
        ApplyUnimodalSecant(in f, in a, ref p2, ref p2ImprovedSecant, ref result);
      }
      else
      {
        _logger.Write("> p2 golden section").NewLine();
        ApplyGoldenSection(in f, in a, in p2, ref result);
      }
      
      if (result.Y.PreciselySmaller(a.Y))
      {
        return result;
      }
      else
      {
        // a is still the smallest, try to narrow the range
        if (p2ImprovedSecant)
        {
          _logger.Write(" Narrowing range to [a, p2)").NewLine();
          return MoveNext(in f, in a, p2.X);
        } 
        else if (p1ImprovedSecant)
        {
          _logger.Write(" Narrowing range to [a, p1)").NewLine();
          return MoveNext(in f, in a, p1.X);
        }
        else if(p1.First < 0)
        {
          _logger.Write(" Narrowing range to [p1, b)").NewLine();
          return MoveNext(in f, in p1, in b);  
        }
        else if(p2.First < 0)
        {
          _logger.Write(" Narrowing range to [p2, b)").NewLine();
          return MoveNext(in f, in p2, in b);  
        }
        else
        {
          // both are zero, let's just pick the winner
          NumberFunctionPointWithDerivative smallerP = p1.Y < p2.Y ? p1 : p2;
          if(a.Y < smallerP.Y)
            smallerP = a;
          return smallerP;
        }
      }
    }
  }
}