using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Analysis.Optimization.Test
{
  public static class OptimizationHelper
  {
    public static readonly ReadOnlyMemory<Number> DirectionDerivativeRatiosD1;
    public static readonly ReadOnlyMemory<Number> DirectionDerivativeRatiosD2;

    static OptimizationHelper()
    {
      DirectionDerivativeRatiosD1 = new Number[] { 1 };
      DirectionDerivativeRatiosD2 = new Number[] { Math.Sqrt(2) / 2, Math.Sqrt(2) / 2 };
    }
    
    public static FunctionValueAnalysisForDirection FunctionValueAnalysisFor1D(
      this IFunctionValueAnalysis functionToAnalyse)
    {
      ReadOnlySpan<Number> startPoint = new Number[] { 0 };
      FunctionValueAnalysisForDirection function = new(
        functionToAnalyse, 
        in startPoint, 
        DirectionDerivativeRatiosD1.Span);
      return function;
    }
    
    public static ushort FindOptimal(
      this INumberFunctionOptimizationForSearchRange method,
      in FunctionValueAnalysisForDirection functionToAnalyse,
      ref NumberFunctionOptimizationSearchRange point)
    {
      ushort i = 0;
      while(!point.IsOptimal)
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
      ushort i = 0;
      bool isTheEnd = false;
      while(!isTheEnd)
      {
        Number width = b - a.X;
        Number value = a.Y;
        
        isTheEnd = !method.MoveNext(functionToAnalyse, ref a, in b);
        i++;

        (b - a.X).AssertIsLessThan(in width);
        a.Y.AssertIsLessEqualThan(in value);
      }

      solution = a;
      return i;
    }
  }
}