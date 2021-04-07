using System;
using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Export;

namespace Arnible.MathModeling.Analysis.Optimization.SingleStep
{
  public class GoldenSectionSmoothlyConstrained : ISingleStepOptimization
  {
    public const double Ratio = 0.618;
    
    private readonly IMathModelingLogger _logger;
    public uint IterationLimit { get; set; }
    
    public GoldenSectionSmoothlyConstrained(
      IMathModelingLogger logger)
    {
      _logger = logger;
      IterationLimit = 10;
    }

    public Number Optimize(
      INumberFunctionWithDerivative f,
      in FunctionPointWithDerivative a,
      in Number b)
    {
      if (a.X == b)
      {
        throw new NotAbleToOptimizeException();
      }

      var aSign = a.First.GetSign();
      if (a.X < b)
      {
        if (aSign != Sign.Negative)
        {
          throw new NotAbleToOptimizeException();
        }
      }
      else
      {
        if (aSign != Sign.Positive)
        {
          throw new NotAbleToOptimizeException();
        }
      }

      return Optimize(0, f, in a, in b);
    }

    private Number Optimize(
      uint iteration,
      INumberFunctionWithDerivative f,
      in FunctionPointWithDerivative a,
      in Number b)
    {
      if (iteration > IterationLimit)
      {
        throw new NotAbleToOptimizeException();
      }
      
      Number width = b - a.X;
      if (width == 0)
      {
        throw new NotAbleToOptimizeException();
      }
      
      FunctionPointWithDerivative p1 = f.ValueWithDerivative(b - Ratio * width);
      FunctionPointWithDerivative p2 = f.ValueWithDerivative(a.X + Ratio * width);
      FunctionPointWithDerivative pMinValue = p1.Y < p2.Y ? p1 : p2;
      
      // try to apply secant method
      FunctionPointWithDerivative? pSecant = ApplyUnimodalSecantIfPossible(f, in a, in p1, in p2);
      if (pSecant.HasValue)
      {
        if (pSecant.Value.Y < pMinValue.Y)
        {
          _logger.Log($"  [{iteration.ToString()}] Secant {pSecant.Value.ToStringValue()} with [{a.ToStringValue()}, {p1.ToStringValue()}, {p2.ToStringValue()}]");
          return pSecant.Value.X;
        }
      }

      // well, maybe we made some progress with golden ratio
      if (pMinValue.Y < a.Y)
      {
        _logger.Log($"  [{iteration.ToString()}] Golden section {pMinValue.X.ToStringValue()} with [{a.ToStringValue()}, {p1.ToStringValue()}, {p2.ToStringValue()}]");
        return pMinValue.X;
      }
      
      // not much luck, let's focus on first range
      return Optimize(iteration + 1, f, in a, p1.X);
    }

    static FunctionPointWithDerivative? ApplyUnimodalSecantIfPossible(
      INumberFunctionWithDerivative f,
      in FunctionPointWithDerivative a,
      in FunctionPointWithDerivative p1,
      in FunctionPointWithDerivative p2)
    {
      UnimodalSecantAnalysis r1Secant = UnimodalSecant.AnalyseApplicability(in a, in p1);
      if (r1Secant == UnimodalSecantAnalysis.HasMinimum)
      {
        return UnimodalSecant.CalculateMinimum(f, in a, in p1);
      }
      else
      {
        UnimodalSecantAnalysis r2Secant = UnimodalSecant.AnalyseApplicability(in p1, in p2);
        if (r2Secant == UnimodalSecantAnalysis.HasMinimum)
        {
          return UnimodalSecant.CalculateMinimum(f, in p1, in p2);
        }
      }
      return null;
    }
  }
}