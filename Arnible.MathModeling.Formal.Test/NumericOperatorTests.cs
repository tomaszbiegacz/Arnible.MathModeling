using System;
using Arnible.Assertions;
using Xunit;

namespace Arnible.MathModeling.Test
{
  public class NumericOperatorTests
  {
    [Fact]
    public void Equals_Infinity_Infinity_Not()
    {
      double.PositiveInfinity.NumericEquals(double.PositiveInfinity).AssertIsFalse();
    }    
    
    [Theory]
    [InlineData(Math.PI, 0, 1)]
    [InlineData(2, 1, 2)]
    [InlineData(2, 2, 4)]
    [InlineData(2, 3, 8)]
    [InlineData(2, 4, 16)]
    [InlineData(2, 5, 32)]
    [InlineData(2, 6, 64)]
    public void Power_0(double value, ushort power, double result)
    {
      value.ToPower(power).AssertIsEqualTo(result);
    }    
  }
}