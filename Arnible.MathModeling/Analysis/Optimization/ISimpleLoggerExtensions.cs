using System;

namespace Arnible.MathModeling.Analysis.Optimization
{
  public static class ISimpleLoggerExtensions
  {
    public static ISimpleLogger Write(
      this ISimpleLogger logger,
      in ReadOnlySpan<char> str0,
      in NumberFunctionPointWithDerivative point)
    {
      return logger
        .Write(in str0)
        .Write("{", point.X)
        .Write("(", point.Y)
        .Write(", ", point.First)
        .Write(")}");
    }
  }
}