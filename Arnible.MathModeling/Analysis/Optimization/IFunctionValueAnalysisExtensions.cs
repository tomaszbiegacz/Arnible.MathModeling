using System;
using Arnible.Linq;

namespace Arnible.MathModeling.Analysis.Optimization
{
  public static class IFunctionValueAnalysisExtensions
  {
    public static bool IsOptimum(this IFunctionValueAnalysis function, in ReadOnlySpan<Number> arguments)
    {
      Span<Number> gradient = stackalloc Number[arguments.Length];
      function.GradientByArguments(in arguments, in gradient);
      return gradient.IsZero();
    }
  }
}