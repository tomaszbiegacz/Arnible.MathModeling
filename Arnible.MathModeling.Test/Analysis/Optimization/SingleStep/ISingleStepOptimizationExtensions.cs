using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization.SingleStep.Test
{
  public static class ISingleStepOptimizationExtensions
  {
    public static ushort FindOptimal(
      this ISingleStepOptimization method,
      in FunctionValueAnalysisForDirection functionToAnalyse,
      ref NumberFunctionPointWithDerivative a,
      in Number b)
    {
      bool useRange = false;
      NumberFunctionOptimizationSearchRange searchRange = default;
      
      ushort i = 0;
      while(!useRange && a.First != 0 || useRange && !searchRange.IsEmptyRange)
      {
        if (useRange)
        {
          Number width = searchRange.Width;
          Number value = searchRange.BorderSmaller.Y;
          
          method.MoveNext(in functionToAnalyse, ref searchRange);
          searchRange.Width.AssertIsLessThan(in width);
          searchRange.BorderSmaller.Y.AssertIsLessEqualThan(in value);
        }
        else
        {
          NumberFunctionPointWithDerivative result = method.MoveNext(in functionToAnalyse, in a, in b, out _);
          result.X.AssertIsGreaterEqualThan(a.X);
          result.Y.AssertIsLessEqualThan(a.Y);
          
          if (result.First <= 0 && result.X != a.X)
          {
            a = result;  
          }
          else
          {
            searchRange = new(in a, in result);
            useRange = true;
          }
        }
        i++;
      }

      if (useRange)
      {
        a = searchRange.BorderSmaller;
      }
      return i;
    }
  }
}