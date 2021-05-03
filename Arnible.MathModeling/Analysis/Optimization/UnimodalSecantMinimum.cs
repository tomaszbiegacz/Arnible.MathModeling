using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization
{
  public class UnimodalSecant
  {
    public static UnimodalSecantAnalysis AnalyseApplicability(
      in NumberFunctionPointWithDerivative a,
      in NumberFunctionPointWithDerivative b)
    {
      a.X.AssertIsLessThan(b.X);
      Sign aSign = a.First.GetSign();
      Sign bSign = b.First.GetSign();
      if (aSign == Sign.Negative && bSign == Sign.Positive)
      {
        return UnimodalSecantAnalysis.HasMinimum;
      }
      else if (aSign == Sign.Positive && bSign == Sign.Negative)
      {
        return UnimodalSecantAnalysis.HasMaximum;
      }
      else
      {
        return UnimodalSecantAnalysis.Unknown;
      }
    }

    private readonly ISimpleLogger _logger;

    public UnimodalSecant(ISimpleLogger logger)
    {
      _logger = logger;
    }

    public void MoveNext(ref NumberFunctionOptimizationKernelForSecant point)
    {
      point.IsOptimal.AssertIsFalse();

      NumberFunctionPointWithDerivative c = point.CalculateOptimum();
      if (c.First == 0)
      {
        if (c.Y > point.Y)
        {
          point.Log(_logger, "Stop, found maximum", in c);
          throw new PolimodalFunctionException();
        }
        else
        {
          point.Log(_logger, "Found minimum", in c);
          point.BorderDecreasing = c;
          point.BorderIncreasing = c;
        }
      }
      else
      {
        // mandatory: d_a < 0 and d_b > 0
        // stop if found not to be unimodal
        if (c.First > 0)
        {
          if (c.Y > point.BorderIncreasing.Y)
          {
            point.Log(_logger, "Stop, not unimodal at b", in c);
            throw new PolimodalFunctionException();
          }
          else
          {
            point.Log(_logger, "Moving point with positive derivative", in c);
            point.BorderIncreasing = c;
          }
        }
        else
        {
          if (c.Y > point.BorderDecreasing.Y)
          {
            point.Log(_logger, "Stop, not unimodal at a", in c);
            throw new PolimodalFunctionException();
          }
          else
          {
            point.Log(_logger, "Moving point with negative derivative", in c);
            point.BorderDecreasing = c;
          }
        }
      }
    }
  }
}