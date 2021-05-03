using System;

namespace Arnible.MathModeling.Analysis.Optimization.SingleStep
{
  public class GoldenSectionSmoothlyConstrainedMinimum : ISingleStepOptimization
  {
    public const double Ratio = 0.618;
    
    private readonly ISimpleLogger _logger;
    private readonly GoldenSectionWithDerivativeConstrainedMinimum _goldenSection;
    private readonly UnimodalSecantMinimum _secant; 
    public uint IterationLimit { get; set; }
    
    public GoldenSectionSmoothlyConstrainedMinimum(ISimpleLogger logger)
    {
      _logger = logger;
      _goldenSection = new GoldenSectionWithDerivativeConstrainedMinimum(logger);
      _secant = new UnimodalSecantMinimum(logger);
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
      
      NumberFunctionPointWithDerivative p1 = f.ValueWithDerivative(b - Ratio * width);
      NumberFunctionPointWithDerivative p2 = f.ValueWithDerivative(a.X + Ratio * width);
      NumberFunctionPointWithDerivative pMinValue = p1.Y < p2.Y ? p1 : p2;
      
      // try to apply secant method
      if (ApplyUnimodalSecantIfPossible(f, in a, in p1, in p2, out NumberFunctionPointWithDerivative pSecant))
      {
        if (pSecant.Y < pMinValue.Y)
        {
          Log(iteration, "Secant", pSecant, in a, in p1, in p2);
          return pSecant.X;
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

    private bool ApplyUnimodalSecantIfPossible(
      INumberFunctionWithDerivative f,
      in NumberFunctionPointWithDerivative a,
      in NumberFunctionPointWithDerivative p1,
      in NumberFunctionPointWithDerivative p2,
      out NumberFunctionPointWithDerivative result)
    {
      NumberFunctionOptimizationSearchRange r1 = new NumberFunctionOptimizationSearchRange(f, in a, in p1);
      if (r1.GetSecantApplicability() == UnimodalSecantAnalysis.HasMinimum)
      {
        _secant.MoveNext(ref r1);
        result = r1.BorderSmaller; 
        return true;
      }
      else
      {
        NumberFunctionOptimizationSearchRange r2 = new NumberFunctionOptimizationSearchRange(f, in p1, in p2);
        if (r2.GetSecantApplicability() == UnimodalSecantAnalysis.HasMinimum)
        {
          _secant.MoveNext(ref r2);
          result = r2.BorderSmaller; 
          return true;
        }
        else
        {
          result = default;
          return false;
        }
      }
    }
    
    protected void Log(
      ushort iteration,
      string message,
      in NumberFunctionPointWithDerivative result,
      in NumberFunctionPointWithDerivative a,
      in NumberFunctionPointWithDerivative p1,
      in NumberFunctionPointWithDerivative p2)
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