using System;

namespace Arnible.MathModeling.Analysis.Optimization.SingleStep
{
  public class GoldenSectionSmoothlyConstrained : ISingleStepOptimization
  {
    public const double Ratio = 0.618;
    
    private readonly ISimpleLogger _logger;
    public uint IterationLimit { get; set; }
    
    public GoldenSectionSmoothlyConstrained(
      ISimpleLogger logger)
    {
      _logger = logger;
      IterationLimit = 10;
    }

    public Number Optimize(
      INumberFunctionWithDerivative f,
      in NumberFunctionPointWithDerivative startPoint,
      in Number maxX)
    {
      var aSign = startPoint.First.GetSign();
      if (startPoint.X < maxX)
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

      return Optimize(
        iteration: 0, 
        f: f, 
        a: in startPoint, 
        b: in maxX);
    }

    private Number Optimize(
      ushort iteration,
      INumberFunctionWithDerivative f,
      in NumberFunctionPointWithDerivative a,
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
          Log(iteration, "Secant", pSecant.Value, in a, in p1, in p2);
          return pSecant.Value.X;
        }
      }

      // well, maybe we made some progress with golden ratio
      if (pMinValue.Y < a.Y)
      {
        Log(iteration, "Golden section", pMinValue, in a, in p1, in p2);
        return pMinValue.X;
      }
      
      // not much luck, let's focus on first range
      return Optimize((ushort)(iteration + 1), f, in a, p1.X);
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
    
    protected void Log(
      ushort iteration,
      string message,
      in FunctionPointWithDerivative result,
      in FunctionPointWithDerivative a,
      in FunctionPointWithDerivative p1,
      in FunctionPointWithDerivative p2)
    {
      Span<char> iterationBuffer = stackalloc char[SpanCharFormatter.BufferSize];
      SpanCharFormatter.ToString(iteration, in iterationBuffer);
      
      _logger.Write("  [", iterationBuffer, "] ", message, " ");
      result.Write(_logger);
      _logger.Write(" with [");
      a.Write(_logger);
      _logger.Write(", ");
      p1.Write(_logger);
      _logger.Write(", ");
      p2.Write(_logger);
      _logger.Write("]");
    }
  }
}