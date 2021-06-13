using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization
{
  public class UnimodalSecantMinimum : INumberFunctionOptimizationForSearchRange
  {
    private readonly ISimpleLogger _logger;

    public UnimodalSecantMinimum(ISimpleLogger logger)
    {
      _logger = logger;
    }
    
    public void MoveNext(
      in FunctionValueAnalysisForDirection functionToAnalyse,
      ref NumberFunctionOptimizationSearchRange point)
    {
      if (!TryMoveNext(in functionToAnalyse, ref point))
      {
        throw new MultimodalFunctionException();
      }
    }

    public bool TryMoveNext(
      in FunctionValueAnalysisForDirection functionToAnalyse,
      ref NumberFunctionOptimizationSearchRange point)
    {
      point.IsOptimal.AssertIsFalse();
      _logger
        .Write("start: ", point.Start)
        .Write(", end: ", point.End)
        .NewLine();

      NumberFunctionPointWithDerivative c = point.CalculateMinimumCandidate(in functionToAnalyse);
      if (c.First == 0)
      {
        if (c.Y > point.BorderSmaller.Y)
        {
          point.Log(_logger, "Stop, found maximum", in c);
          return false;
        }
        else
        {
          point.Log(_logger, "Found minimum", in c);
          point.BorderLowestDerivative = c;
          point.BorderHighestDerivative = c;
          return true;
        }
      }
      else
      {
        // mandatory: d_a < 0 and d_b > 0
        // stop if found not to be unimodal
        if (c.First > 0)
        {
          if (c.Y > point.BorderHighestDerivative.Y || c.X < point.BorderLowestDerivative.X)
          {
            point.Log(_logger, "Stop, not unimodal at b", in c);
            return false;
          }
          else
          {
            point.Log(_logger, "Moving point with positive derivative", in c);
            point.BorderHighestDerivative = c;
            return true;
          }
        }
        else
        {
          if (c.Y > point.BorderLowestDerivative.Y || c.X > point.BorderHighestDerivative.X)
          {
            point.Log(_logger, "Stop, not unimodal at a", in c);
            return false;
          }
          else
          {
            point.Log(_logger, "Moving point with negative derivative", in c);
            point.BorderLowestDerivative = c;
            return true;
          }
        }
      }
    }
  }
}