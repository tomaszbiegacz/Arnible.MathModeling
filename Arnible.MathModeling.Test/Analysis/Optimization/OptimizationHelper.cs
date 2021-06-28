using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization.Test
{
  public static class OptimizationHelper
  {
    public static readonly ReadOnlyMemory<Number> UniformDirectionRatiosD1;
    public static readonly ReadOnlyMemory<Number> UniformDirectionRatiosD2;

    static OptimizationHelper()
    {
      UniformDirectionRatiosD1 = new Number[] { 1 };
      UniformDirectionRatiosD2 = new Number[] { Math.Sqrt(2) / 2, Math.Sqrt(2) / 2 };
    }
    
    public static FunctionValueAnalysisForDirection FunctionValueAnalysisFor1D(
      this IFunctionValueAnalysis functionToAnalyse)
    {
      ReadOnlySpan<Number> startPoint = new Number[] { 0 };
      FunctionValueAnalysisForDirection function = new(
        functionToAnalyse, 
        in startPoint, 
        UniformDirectionRatiosD1.Span);
      return function;
    }
    
    public static ushort FindOptimal(
      this INumberFunctionOptimizationForSearchRange method,
      in FunctionValueAnalysisForDirection functionToAnalyse,
      ref NumberFunctionOptimizationSearchRange point)
    {
      ushort i = 0;
      while(!point.IsEmptyRange)
      {
        Number width = point.Width;
        Number value = point.BorderSmaller.Y;
        
        method.MoveNext(in functionToAnalyse, ref point);
        i++;
        
        point.Width.AssertIsLessThan(in width);
        point.BorderSmaller.Y.AssertIsLessEqualThan(in value);
      }

      return i;
    }
    
    public static ushort FindOptimal(
      this INumberFunctionOptimizationForSmoothSearchRange method,
      in FunctionValueAnalysisForDirection functionToAnalyse,
      NumberFunctionPointWithDerivative a,
      Number b,
      out NumberFunctionPointWithDerivative solution)
    {
      solution = default;
      
      ushort i = 0;
      NumberFunctionOptimizationSearchRange searchRange = default;
      bool useSearchRange = false;
      bool isTheEnd = false;
      while(!isTheEnd)
      {
        i++;
        
        if(useSearchRange)
        {
          Number width = searchRange.Width;
          Number value = searchRange.BorderSmaller.Y;
        
          method.MoveNext(in functionToAnalyse, ref searchRange);

          searchRange.Width.AssertIsLessThan(in width);
          searchRange.BorderSmaller.Y.AssertIsLessEqualThan(in value);
          
          if (searchRange.IsEmptyRange)
          {
            isTheEnd = true;
            solution = searchRange.BorderSmaller;
          }
        }
        else
        {
          Number width = b - a.X;
          Number value = a.Y;
          
          bool hasImproved = method.MoveNext(functionToAnalyse, ref a, in b, out searchRange);
          if(hasImproved)
          {
            (b - a.X).AssertIsLessThan(in width);
            a.Y.AssertIsLessEqualThan(in value);
            
            // optimum seems to be on b, or at least nearby
            if (a.X == b)
            {
              isTheEnd = true;
              solution = a;
            }
          }
          else
          {
            useSearchRange = true;
            if (searchRange.IsEmptyRange)
            {
              isTheEnd = true;
              solution = searchRange.BorderSmaller; 
            }
          }
        }
      }
      
      return i;
    }
  }
}