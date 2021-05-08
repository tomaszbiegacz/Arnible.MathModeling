using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization
{
  static class UnimodalSecant
  {
    public static UnimodalSecantAnalysis GetSecantApplicability(this in NumberFunctionOptimizationSearchRange src)
    {
      Sign aSign = src.Start.First.GetSign();
      Sign bSign = src.End.First.GetSign();
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
    
    public static NumberFunctionPointWithDerivative CalculateOptimum(
      this in NumberFunctionOptimizationSearchRange src,
      in FunctionValueAnalysisForDirection functionToAnalyse)
    {
      NumberFunctionPointWithDerivative a = src.Start;
      NumberFunctionPointWithDerivative b = src.End;
      
      a.First.GetSign().AssertIsNotEqualToEnum(b.First.GetSign());
      // a.First < 0 && b.First > 0 => minimum
      // a.First > 0 && b.First < 0 => maximum
      
      Number step = a.First * (b.X - a.X) / (b.First - a.First);
      step.AssertIsBetween(a.X - b.X, 0);

      return functionToAnalyse.ValueWithDerivative(a.X - step);
    }
  }
}