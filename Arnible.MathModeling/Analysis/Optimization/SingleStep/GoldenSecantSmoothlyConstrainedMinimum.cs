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

    public void MoveNext(
      in FunctionValueAnalysisForDirection f,
      ref NumberFunctionOptimizationSearchRange searchRange)
    {
      searchRange.Start.First.AssertIsLessThan(0);
      
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
      return MoveNext(0, in f, in a, in b);
    }

    private NumberFunctionPointWithDerivative MoveNext(
      uint iteration,
      in FunctionValueAnalysisForDirection f,
      in NumberFunctionPointWithDerivative a,
      in Number b)
    {
      a.First.AssertIsLessThan(0);
      b.AssertIsGreaterThan(a.X);
      
      _logger
        .Write("[", iteration)
        .Write("] MoveNext ", in a)
        .Write(" to ", in b)
        .NewLine();

      NumberFunctionPointWithDerivative result = a;
      Number width = b - a.X;
      NumberFunctionPointWithDerivative p1 = f.ValueWithDerivative(b - Ratio * width);
      Sign p1FirstSign = p1.First.GetSign();
      
      _logger.Write("p: ", in p1).NewLine();
      if(a.X == p1.X)
      {
        _logger.Write("We are on the edge").NewLine();
        if(p1.Y < a.Y)
          return p1;
        else
          return a;
      }
      else if(p1FirstSign is Sign.Negative)
      {
        if(p1.Y < a.Y)
        {
          _logger.Write("p1First is negative and value is lower; let's finish here").NewLine();
          return p1;
        }
        else
        {
          _logger.Write("p1First is negative, but value is greater; let's focus on nearest minimum").NewLine();
          return MoveNext(iteration + 1, in f, in a, p1.X);  
        }
      }
      else
      {
        if(p1FirstSign is Sign.Positive)
        {
          _logger.Write("> p1 secant").NewLine();
          ApplyUnimodalSecant(in f, in a, in p1, ref result);
        }
        if(p1FirstSign is Sign.Positive or Sign.None)
        {
          _logger.Write("> p1 golden section").NewLine();
          ApplyGoldenSection(in f, in a, in p1, ref result);
        }
        
        if(result.Y < a.Y)
        {
          // we are done in this iteration
          return result;
        }
        else
        {
          // a.First is < 0
          return MoveNext(iteration + 1, in f, in a, p1.X);
        }
      }
    }
    
    private void ApplyUnimodalSecant(
      in FunctionValueAnalysisForDirection f,
      in NumberFunctionPointWithDerivative a,
      in NumberFunctionPointWithDerivative b,
      ref NumberFunctionPointWithDerivative result)
    {
      NumberFunctionOptimizationSearchRange r = new(in a, in b);
      if(_secant.TryMoveNext(in f, ref r))
      {
        if (r.BorderSmaller.Y < result.Y)
        {
          result = r.BorderSmaller;  
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
  }
}