using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization
{
  public class GoldenSectionConstrained : INumberFunctionOptimization
  {
    public const double Ratio = 0.618;
    private readonly ISimpleLogger _logger;
    
    public GoldenSectionConstrained(ISimpleLogger logger)
    {
      _logger = logger;
    }
    
    public bool MoveNext(ref NumberFunctionOptimizationKernel point)
    {
      if (point.IsOptimal)
      {
        // we're done here
        return false;
      }

      NumberFunctionPointWithDerivative a = point.BorderSmaller;
      NumberFunctionPointWithDerivative b = point.BorderGreater;
      Number width = point.Width;
      
      Number x1 = point.BorderSmaller.X + Ratio * width;
      Number x2 = point.BorderGreater.X - Ratio * width;
      
      _logger.Log("> x1");
      MoveNext(ref point, in a, in b, point.F.ValueWithDerivative(in x1));
      _logger.Log("> x2");
      MoveNext(ref point, in a, in b, point.F.ValueWithDerivative(in x2));

      return true;
    }

    protected void MoveNext(
      ref NumberFunctionOptimizationKernel point,
      in NumberFunctionPointWithDerivative a,
      in NumberFunctionPointWithDerivative b,
      in NumberFunctionPointWithDerivative c)
    {
      Sign daSign = a.First.GetSign();
      Sign dbSign = b.First.GetSign();
      Sign dcSign = c.First.GetSign();
      
      // mandatory: a.Y <= b.Y
      // assume that it is unimodal, but be tolerant. Ideally a.First != b.First or a.First == b.First == 0
      if (c.Y < a.Y)
      {
        // smallest value has just been improved
        if (dcSign != dbSign || dbSign == Sign.None)
        {
          point.Log(_logger, "Improving a with dc != db or db = 0", in c);
          point.BorderSmaller = c;
        }
        else
        {
          // dcSign == dbSign
          if (daSign != dcSign)
          {
            point.Log(_logger, "Focusing on first section having smallest, values with c being a", in c);
            point.BorderGreater = point.BorderSmaller;
            point.BorderSmaller = c;  
          }
          else
          {
            point.Log(_logger, "Focusing on first section having smallest, values dcSign = dbSign", in c);
            point.BorderSmaller = c;
          }
        }
      }
      else
      {
        if (c.Y < b.Y)
        {
          // smallest value is still a, but b has been improved
          if(dcSign != daSign || daSign == Sign.None)
          {
            point.Log(_logger, "Improving b with dc != da or da == 0", in c);
            point.BorderGreater = c;
          }
          else
          {
            point.Log(_logger, "Focusing on first section having smallest values", in c);
            point.BorderGreater = c;
          }
        }
        else
        {
          if (c.Y > b.Y)
          {
            point.Log(_logger, "Narrowing search to first section", in c);
            point.BorderGreater = c;  
          }
          else
          {
            // c is equal to b and a 
            if (dbSign == dcSign)
            {
              point.Log(_logger, "Narrowing search to first section, with dbSign = dcSign", in c);
              point.BorderGreater = c;
            }
            else
            {
              if (daSign == dcSign)
              {
                point.Log(_logger, "Narrowing search to second section, with daSign = dcSign", in c);
                point.BorderSmaller = c;
              }
              else
              {
                if (daSign == Sign.Negative || daSign == Sign.None)
                {
                  point.Log(_logger, "Narrowing search to first section, with daSign <= 0", in c);
                  point.BorderGreater = c;
                }
                else
                {
                  point.Log(_logger, "Narrowing search to second section, with daSign > 0", in c);
                  point.BorderSmaller = c;
                }
              }
            }
          }
        }
      }

      point.BorderGreater.Y.AssertIsGreaterEqualThan(point.BorderSmaller.Y);
    }
  }
}