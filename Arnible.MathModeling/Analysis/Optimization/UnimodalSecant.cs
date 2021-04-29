using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization
{
  public class UnimodalSecant : IUnimodalOptimization
  {
    public static UnimodalSecantAnalysis AnalyseApplicability(
      in FunctionPointWithDerivative a,
      in FunctionPointWithDerivative b)
    {
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

    public static FunctionPointWithDerivative CalculateMinimum(
      INumberFunctionWithDerivative f,
      in FunctionPointWithDerivative a,
      in FunctionPointWithDerivative b)
    {
      a.First.AssertIsLessThan(0);
      b.First.AssertIsGreaterThan(0);

      Number step = a.First * (b.X - a.X) / (b.First - a.First);
      step.AssertIsNotEqualTo(0);

      return f.ValueWithDerivative(a.X - step);
    }
    
    private readonly INumberFunctionWithDerivative _f;
    private FunctionPointWithDerivative _a;
    private FunctionPointWithDerivative _b;
    private readonly ISimpleLogger _logger;

    public UnimodalSecant(
      INumberFunctionWithDerivative f,
      in FunctionPointWithDerivative a,
      in FunctionPointWithDerivative b,
      ISimpleLogger logger)
    {
      if (a.First < 0)
      {
        _a = a;
        _b = b;  
      }
      else
      {
        _a = b;
        _b = a;
      }
      if (_a.First >= 0 || _b.First <= 0)
      {
        throw new ArgumentException();
      }
      
      _f = f;
      
      IsPolimodal = false;
      _logger = logger;
    }

    public bool IsOptimal => _a.X == _b.X;
    public bool IsPolimodal { get; private set; }

    public Number X => _a.Y < _b.Y ? _a.X : _b.X;
    public Number Y => _a.Y < _b.Y ? _a.Y : _b.Y;

    public Number Width => (_b.X - _a.X).Abs();

    public bool MoveNext()
    {
      if (IsOptimal || IsPolimodal)
      {
        return false;
      }

      FunctionPointWithDerivative c = CalculateMinimum(_f, in _a, in _b);
      if (c.First == 0)
      {
        if (c.Y > Y)
        {
          Log("Stop, found maximum", in c);
          IsPolimodal = true;
          return false;
        }
        else
        {
          Log("Found minimum", in c);
          _a = c;
          _b = c;
          return true;
        }
      }
      else
      {
        // mandatory: d_a < 0 and d_b > 0
        // stop if found not to be unimodal
        if (c.First > 0)
        {
          if (c.Y > _b.Y)
          {
            Log("Stop, not unimodal at b", in c);
            IsPolimodal = true;
            return false;
          }
          else
          {
            Log("Moving point with positive derivative", in c);
            _b = c;
            return true;
          }
        }
        else
        {
          if (c.Y > _a.Y)
          {
            Log("Stop, not unimodal at a", in c);
            IsPolimodal = true;
            return false;
          }
          else
          {
            Log("Moving point with negative derivative", in c);
            _a = c;
            return true;
          }
        }
      }
    }
    
    private void Log(
      string message,
      in FunctionPointWithDerivative c)
    {
      _logger.Log($"  [{_a.ToStringValue()}, {_b.ToStringValue()}] {message}, c:{c.ToStringValue()}");
    }
  }
}