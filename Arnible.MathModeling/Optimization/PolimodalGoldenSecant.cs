using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Export;

namespace Arnible.MathModeling.Optimization
{
  public class PolimodalGoldenSecant : GoldenSectionConstrained
  {
    public PolimodalGoldenSecant(
      INumberFunctionWithDerivative f,
      in FunctionPointWithDerivative a,
      in FunctionPointWithDerivative b,
      IMathModelingLogger logger)
    : base (f, in a, in b, logger)
    {
      // intentionally empty
    }

    private bool TryUnimodalSecant()
    {
      Sign d1Sign = BorderSmaller.First.GetSign();
      Sign d2Sign = BorderGreater.First.GetSign();
      if (d1Sign == d2Sign || d1Sign == Sign.None || d2Sign == Sign.None)
      {
        // this is not a good candidate for secant method
        return false;
      }
      
      FunctionPointWithDerivative a;
      FunctionPointWithDerivative b;
      if (d1Sign < d2Sign)
      {
        a = BorderSmaller;
        b = BorderGreater;
      }
      else
      {
        a = BorderGreater;
        b = BorderSmaller;
      }
      FunctionPointWithDerivative c = UnimodalSecant.CalculateMinimum(F, in a, in b);
      if (c.First == 0)
      {
        if (c.Y > Y)
        {
          Log("Secant found maximum", in c);
          return false;
        }
        else
        {
          Log("Found minimum", in c);
          Update(in c, in c);
          return true;
        }
      }
      else
      {
        if (c.First > 0)
        {
          if (c.Y > b.Y)
          {
            Log("Not unimodal at b", in c);
            return false;
          }
          else
          {
            Log("Moving point with positive derivative", in c);
            Update(in a, in c);
            return true;
          }
        }
        else
        {
          if (c.Y > a.Y)
          {
            Log("Stop, not unimodal at a", in c);
            return false;
          }
          else
          {
            Log("Moving point with negative derivative", in c);
            Update(in c, in b);
            return true;
          }
        } 
      }
    }

    public override bool MoveNext()
    {
      if (IsOptimal)
      {
        return false;
      }
      
      if (TryUnimodalSecant())
      {
        return true;
      }
      else
      {
        return base.MoveNext();  
      }
    }
  }
}