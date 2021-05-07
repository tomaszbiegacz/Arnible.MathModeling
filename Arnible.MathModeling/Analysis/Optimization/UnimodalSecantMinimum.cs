using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization
{
  public class UnimodalSecantMinimum : INumberFunctionOptimization
  {
    private readonly ISimpleLogger _logger;

    public UnimodalSecantMinimum(ISimpleLogger logger)
    {
      _logger = logger;
    }

    public void MoveNext(ref NumberFunctionOptimizationSearchRange point)
    {
      point.IsOptimal.AssertIsFalse();

      NumberFunctionPointWithDerivative c = point.CalculateOptimum();
      if (c.First == 0)
      {
        if (c.Y > point.BorderSmaller.Y)
        {
          point.Log(_logger, "Stop, found maximum", in c);
          throw new MultimodalFunctionException();
        }
        else
        {
          point.Log(_logger, "Found minimum", in c);
          point.BorderLowestDerivative = c;
          point.BorderGreatestDerivative = c;
        }
      }
      else
      {
        // mandatory: d_a < 0 and d_b > 0
        // stop if found not to be unimodal
        if (c.First > 0)
        {
          if (c.Y > point.BorderGreatestDerivative.Y)
          {
            point.Log(_logger, "Stop, not unimodal at b", in c);
            throw new MultimodalFunctionException();
          }
          else
          {
            point.Log(_logger, "Moving point with positive derivative", in c);
            point.BorderGreatestDerivative = c;
          }
        }
        else
        {
          if (c.Y > point.BorderLowestDerivative.Y)
          {
            point.Log(_logger, "Stop, not unimodal at a", in c);
            throw new MultimodalFunctionException();
          }
          else
          {
            point.Log(_logger, "Moving point with negative derivative", in c);
            point.BorderLowestDerivative = c;
          }
        }
      }
    }
  }
}