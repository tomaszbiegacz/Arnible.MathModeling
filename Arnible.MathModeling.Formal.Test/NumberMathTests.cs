using System;
using Arnible.Assertions;
using Xunit;

namespace Arnible.MathModeling.Test
{
  public class NumberMathTests
  {
    [Theory]
    [InlineData(0, 0)]
    [InlineData(Math.PI / 2, 1)]
    public void RoundedSinTests(double value, double expected)
    {
      NumberMath.RoundedSin(value).AssertIsEqualTo(expected);
    }
    
    [Theory]
    [InlineData(0, 1)]
    [InlineData(Math.PI / 2, 0)]
    public void RoundedCosTests(double value, double expected)
    {
      NumberMath.RoundedCos(value).AssertIsEqualTo(expected);
    }
  }
}