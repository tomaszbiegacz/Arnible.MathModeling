using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization.Test
{
  public static class OptimizationHelper
  {
    public static ushort FindOptimal(
      this INumberFunctionOptimization method,
      ref NumberFunctionOptimizationSearchRange point)
    {
      ushort i = 0;
      while(!point.IsOptimal)
      {
        Number width = point.Width;
        Number value = point.BorderSmaller.Y;
        
        method.MoveNext(ref point);
        i++;
        
        point.Width.AssertIsLessThan(in width);
        point.BorderSmaller.Y.AssertIsLessEqualThan(in value);
      }

      return i;
    }
  }
}