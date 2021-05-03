using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization
{
  /// <summary>
  /// Golden section search for minimum.
  /// Search is capable of dealing with multimodal functions and it uses sign of derivative 
  /// to find section where secant method can be used to speed up optimization task.    
  /// </summary>
  public class GoldenSectionWithDerivativeConstrainedMinimum : INumberFunctionOptimization
  {
    public const double Ratio = 0.618;
    private readonly ISimpleLogger _logger;
    
    public GoldenSectionWithDerivativeConstrainedMinimum(ISimpleLogger logger)
    {
      _logger = logger;
    }
    
    public void MoveNext(ref NumberFunctionOptimizationSearchRange point)
    {
      point.IsOptimal.AssertIsFalse();
      
      Number width = point.Width;
      Number x1 = point.End.X - Ratio * width;
      Number x2 = point.Start.X + Ratio * width;
      
      _logger.Log("> x1");
      MoveNext(ref point, point.ValueWithDerivative(in x1));
      if (point.End.X > x2)
      {
        _logger.Log("> x2");
        MoveNext(ref point, point.ValueWithDerivative(in x2));  
      }
    }

    protected void MoveNext(
      ref NumberFunctionOptimizationSearchRange point,
      in NumberFunctionPointWithDerivative c)
    {
      NumberFunctionPointWithDerivative a = point.BorderSmaller;
      NumberFunctionPointWithDerivative b = point.BorderGreater;
      
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
    }
  }
}