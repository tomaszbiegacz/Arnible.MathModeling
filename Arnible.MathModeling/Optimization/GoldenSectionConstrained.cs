using System;
using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Export;

namespace Arnible.MathModeling.Optimization
{
  public class GoldenSectionConstrained : IUnimodalOptimization
  {
    private const double Ratio = 0.618;
    
    private readonly INumberFunctionWithDerivative _f;
    private ValueWithDerivative1 _a;
    private ValueWithDerivative1 _b;
    private readonly IMathModelingLogger _logger;

    public GoldenSectionConstrained(
      INumberFunctionWithDerivative f,
      ValueWithDerivative1 a,
      ValueWithDerivative1 b,
      IMathModelingLogger logger)
    {
      if (a.X <= b.X)
      {
        _a = a;
        _b = b;  
      }
      else
      {
        _a = b;
        _b = a;
      }
      _f = f;
      _logger = logger;
    }
    
    public bool IsOptimal => _a.X == _b.X || _a.First == 0 || _b.First == 0;
    public Number X => _a.Y < _b.Y ? _a.X : _b.X;
    public Number Y => _a.Y < _b.Y ? _a.Y : _b.Y;

    public Number Width => _b.X - _a.X;

    public bool MoveNext()
    {
      if (IsOptimal)
      {
        // we're done here
        return false;
      }

      Number width = _b.X - _a.X;
      Number x1 = _a.X + Ratio * width;
      Number x2 = _b.X - Ratio * width;
      
      ValueWithDerivative1 c1 = _f.ValueWithDerivative(in x1);
      ValueWithDerivative1 c2 = _f.ValueWithDerivative(in x2);
      
      if (c1.Y < c2.Y)
      {
        Sign d2Sign = c2.First.GetSign();
        Sign dbSign = _b.First.GetSign();
        if (d2Sign >= dbSign)
        {
          _logger.Log($"  Removing last section, from {_b.ToStringValue()} to {c2.ToStringValue()}");
          _b = c2;  
        }
        else
        {
          _logger.Log($"  Focusing on last section, from {_a.ToStringValue()} to {c2.ToStringValue()}");
          _a = c2;
        }
      }
      else
      {
        Sign daSign = _a.First.GetSign();
        Sign d1Sign = c1.First.GetSign();
        if (daSign >= d1Sign)
        {
          _logger.Log($"  Removing first section, from {_a.ToStringValue()} to {c1.ToStringValue()}");
          _a = c1;  
        }
        else
        {
          _logger.Log($"  Focusing on first section, from {_b.ToStringValue()} to {c1.ToStringValue()}");
          _b = c1;
        }
      }
      return true;
    }

    public bool IsSecantOptimizerReady => _a.First.GetSign() != _b.First.GetSign();

    public UnimodalSecant GetSecantOptimizer()
    {
      if (!IsSecantOptimizerReady)
      {
        throw new InvalidOperationException("Not secant ready");
      }
      
      return new UnimodalSecant(f: _f, a: _a, b: _b, _logger);
    }
  }
}