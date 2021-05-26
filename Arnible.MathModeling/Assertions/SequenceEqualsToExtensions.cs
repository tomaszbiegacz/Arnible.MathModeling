using System.Collections.Generic;
using System.Linq;
using Arnible.MathModeling;

namespace Arnible.Assertions
{
  public static class SequenceEqualsToExtensions
  {
    public static void AssertSequenceEqualsTo(this IEnumerable<Number> actual, IReadOnlyList<double> expected)
    {
      actual.AssertSequenceEqualsTo(expected.Select(v => (Number)v).ToArray());
    }
  }
}