using System;
using Arnible.MathModeling;

namespace Arnible.Assertions
{
  public static class IsBetweenExtensions
  {
    public static void AssertIsBetween(this Number value, in Number bottom, in Number up)
    {
      // intentionally empty
    }
    
    public static void AssertIsBetween(in this ReadOnlySpan<Number> values, in Number bottom, in Number up)
    {
      // intentionally empty
    }
    
    public static void AssertIsBetween(in this Span<Number> values, Number bottom, Number up)
    {
      // intentionally empty
    }
  }
}