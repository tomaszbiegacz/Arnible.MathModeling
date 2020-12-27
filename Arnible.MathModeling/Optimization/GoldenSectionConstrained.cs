using System;
using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Export;

namespace Arnible.MathModeling.Optimization
{
  public class GoldenSectionConstrained : IUnimodalOptimization
  {
    public const double Ratio = 0.618;
    
    private NumberValueWithDerivative1 _a;
    private NumberValueWithDerivative1 _b;
    private readonly IMathModelingLogger _logger;

    public GoldenSectionConstrained(
      INumberFunctionWithDerivative f,
      NumberValueWithDerivative1 a,
      NumberValueWithDerivative1 b,
      IMathModelingLogger logger)
    {
      Update(in a, in b);
      F = f;
      _logger = logger;
    }

    protected void Update(in NumberValueWithDerivative1 a, in NumberValueWithDerivative1 b)
    {
      if (a.Y < b.Y)
      {
        _a = a;
        _b = b;  
      }
      else
      {
        _a = b;
        _b = a;
      }
    }

    protected INumberFunctionWithDerivative F { get; }
    protected ref readonly NumberValueWithDerivative1 BorderSmaller => ref _a;
    protected ref readonly NumberValueWithDerivative1 BorderGreater => ref _b;
    
    public bool IsOptimal => _a.X == _b.X;
    
    public Number X => _a.X;
    public Number Y => _a.Y;

    public Number Width => (_b.X - _a.X).Abs();

    public virtual bool MoveNext()
    {
      if (IsOptimal)
      {
        // we're done here
        return false;
      }

      Number width = _b.X - _a.X;
      
      Number x1 = _a.X + Ratio * width;
      Number x2 = _b.X - Ratio * width;
      
      MoveNext("x1", F.ValueWithDerivative(in x1));
      MoveNext("x2", F.ValueWithDerivative(in x2));

      return true;
    }

    private void MoveNext(string prefix, in NumberValueWithDerivative1 c)
    {
      Sign daSign = _a.First.GetSign();
      Sign dbSign = _b.First.GetSign();
      Sign dcSign = c.First.GetSign();
      
      // mandatory: a.Y <= b.Y
      // assume that it is unimodal, but be tolerant. Ideally a.First != b.First or a.First == b.First == 0
      if (c.Y < _a.Y)
      {
        // smallest value has just been improved
        if (dcSign != dbSign || dbSign == Sign.None)
        {
          Log($"{prefix} Improving a with dc != db or db = 0", in c);
          _a = c;
        }
        else
        {
          // dcSign == dbSign
          if (daSign != dcSign)
          {
            Log($"{prefix} Focusing on first section having smallest, values with c being a", in c);
            _b = _a;
            _a = c;  
          }
          else
          {
            Log("Focusing on first section having smallest, values dcSign = dbSign", in c);
            _a = c;
          }
        }
      }
      else
      {
        if (c.Y < _b.Y)
        {
          // smallest value is still a, but b has been improved
          if(dcSign != daSign || daSign == Sign.None)
          {
            Log($"{prefix} Improving b with dc != da or da == 0", in c);
            _b = c;
          }
          else
          {
            Log($"{prefix} Focusing on first section having smallest values", in c);
            _b = c;
          }
        }
        else
        {
          if (c.Y > _b.Y)
          {
            Log("Narrowing search to first section", in c);
            _b = c;  
          }
          else
          {
            // c is equal to b and a 
            if (dbSign == dcSign)
            {
              Log("Narrowing search to first section, with dbSign = dcSign", in c);
              _b = c;
            }
            else
            {
              if (daSign == dcSign)
              {
                Log("Narrowing search to second section, with daSign = dcSign", in c);
                _a = c;
              }
              else
              {
                if (daSign == Sign.Negative || daSign == Sign.None)
                {
                  Log("Narrowing search to first section, with daSign <= 0", in c);
                  _b = c;
                }
                else
                {
                  Log("Narrowing search to second section, with daSign > 0", in c);
                  _a = c;
                }
              }
            }
          }
        }
      }

      if (_b.Y < _a.Y)
      {
        throw new InvalidOperationException($"Something went wrong {_a.ToStringValue()}, {_b.ToStringValue()}");
      }
    }

    protected void Log(
      in string message,
      in NumberValueWithDerivative1 c)
    {
      _logger.Log($"  [{_a.ToStringValue()}, {_b.ToStringValue()}] {message}, c:{c.ToStringValue()}");
    }
  }
}