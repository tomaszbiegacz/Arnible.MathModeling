using System;
using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Export;

namespace Arnible.MathModeling.Optimization
{
  public class UnimodalSecant : IUnimodalOptimization
  {
    private readonly INumberFunctionWithDerivative _f;
    private NumberValueWithDerivative1 _a;
    private NumberValueWithDerivative1 _b;
    private bool _notUnimodalFunction;
    private readonly IMathModelingLogger _logger;

    public UnimodalSecant(
      INumberFunctionWithDerivative f,
      NumberValueWithDerivative1 a,
      NumberValueWithDerivative1 b,
      IMathModelingLogger logger)
    {
      if (a.First <= 0)
      {
        _a = a;
        _b = b;  
      }
      else
      {
        _a = b;
        _b = a;
      }
      if (_b.First < 0)
      {
        throw new ArgumentException(nameof(b));
      }
      
      _f = f;
      
      _notUnimodalFunction = false;
      _logger = logger;
    }

    public bool IsOptimal => _a.X == _b.X ||  _a.First == 0 || _b.First == 0;
    public Number X => _a.Y < _b.Y ? _a.X : _b.X;
    public Number Y => _a.Y < _b.Y ? _a.Y : _b.Y;

    public Number Width
    {
      get
      {
        Number value = _b.X - _a.X;
        return value * (int)value.GetSign();
      }
    }

    public bool MoveNext()
    {
      if (_a.First > 0 || _b.First < 0)
      {
        // something when wrong
        throw new InvalidOperationException($"da: {_a.First.ToStringValue()}, db: {_b.First.ToStringValue()}");
      }
      if (IsOptimal)
      {
        // we're done here
        return false;
      }
      if (_a.First == _b.First || _notUnimodalFunction)
      {
        // we've a problem here
        return false;
      }

      Number step = _a.First * (_b.X - _a.X) / (_b.First - _a.First);
      NumberValueWithDerivative1 c = _f.ValueWithDerivative(_a.X - step);
      
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
          _logger.Log($"  Improving b with {c.ToStringValue()} to width {Width.ToStringValue()} having a {_a.ToStringValue()}");
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
          _logger.Log($"  Improving a with {c.ToStringValue()} to width {Width.ToStringValue()} having b {_b.ToStringValue()}");
          return true;
        }
      }
    }
  }
}