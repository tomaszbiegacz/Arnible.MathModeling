using System;
using System.Collections.Generic;
using Arnible.MathModeling;

namespace Arnible.Assertions
{
  public static class SequenceEqualsToExtensions
  {
    public static void AssertSequenceEqualsTo(in this ReadOnlySpan<Number> actual, IReadOnlyList<double> expected)
    {
      actual.AssertSequenceEqualsTo(expected.ToNumberArray());
    }
    
    public static void AssertSequenceEqualsTo(in this Span<Number> actual, IReadOnlyList<double> expected)
    {
      AssertSequenceEqualsTo((ReadOnlySpan<Number>)actual, expected);
    }
  }
}