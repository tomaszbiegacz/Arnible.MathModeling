using System;
using Arnible.MathModeling.Export;

namespace Arnible.MathModeling.Optimization
{
  public class UnimodalSecant : IUnimodalOptimization
  {
    private readonly INumberFunctionWithDerivative _f;
    private ValueWithDerivative1 _a;
    private ValueWithDerivative1 _b;
    private bool _notUnimodalFunction;
    private readonly IMathModelingLogger _logger;

    public UnimodalSecant(
      INumberFunctionWithDerivative f,
      ValueWithDerivative1 a,
      ValueWithDerivative1 b,
      IMathModelingLogger logger)
    {
      if (a.First >= 0)
      {
        throw new ArgumentException(nameof(a));
      }
      if (b.First <= 0)
      {
        throw new ArgumentException(nameof(b));
      }
      
      _f = f;
      _a = a;
      _b = b;
      _notUnimodalFunction = false;
      _logger = logger;
    }
    
    public Number X => _a.Y < _b.Y ? _a.X : _b.X;
    public Number Y => _a.Y < _b.Y ? _a.Y : _b.Y;

    public bool MoveNext()
    {
      if (_a.First > 0 || _b.First <= 0)
      {
        throw new InvalidOperationException($"da: {_a.First.ToStringValue()}, db: {_b.First.ToStringValue()}");
      }
      if (_a.First == 0 || _notUnimodalFunction)
      {
        return false;
      }

      Number step = _a.First * (_b.X - _a.X) / (_b.First - _a.First);
      ValueWithDerivative1 c = _f.ValueWithDerivative(_a.X - step);
      
      if (c.First == 0)
      {
        if (c.Y > Y)
        {
          _logger.Log($"  Function found not to be unimodal due to {_a.ToStringValue()}, {_b.ToStringValue()}, {c.ToStringValue()}");
          _notUnimodalFunction = true;
          return false;
        }
        else
        {
          _logger.Log($"  Function optimum found at {c.ToStringValue()}");
          _a = c;
          return true;
        }
      }

      if (c.First > 0)
      {
        if (c.Y > _b.Y)
        {
          _logger.Log($"  Function found not to be unimodal due to {_a.ToStringValue()}, {_b.ToStringValue()}, {c.ToStringValue()}");
          _notUnimodalFunction = true;
          return false;
        }
        else
        {
          _b = c;
          _logger.Log($"  Improving b with {c.ToStringValue()}");
          return true;
        }
      }
      else
      {
        if (c.Y > _a.Y)
        {
          _logger.Log($"  Function found not to be unimodal due to {_a.ToStringValue()}, {_b.ToStringValue()}, {c.ToStringValue()}");
          _notUnimodalFunction = true;
          return false;
        }
        else
        {
          _a = c;
          _logger.Log($"  Improving a with {c.ToStringValue()}");
          return true;
        }
      }
    }
  }
}