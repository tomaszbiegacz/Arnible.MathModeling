using Xunit;

namespace Arnible.MathModeling.Test
{
  public class NumericOperatorTests
  {
    [Fact]
    public void IsValidNumeric_Special()
    {
      Assert.False(double.NaN.IsValidNumeric());
      Assert.False(double.PositiveInfinity.IsValidNumeric());
      Assert.False(double.NegativeInfinity.IsValidNumeric());
    }

    [Fact]
    public void IsValidNumeric_Valid()
    {
      Assert.True(0d.IsValidNumeric());
    }

    [Fact]
    public void Equals_Infinity_Infinity_Not()
    {
      Assert.False(NumericOperator.Equals(double.PositiveInfinity, double.PositiveInfinity));
    }

    [Fact]
    public void Equals_NaN_NaN_Not()
    {
      Assert.False(NumericOperator.Equals(double.PositiveInfinity, double.PositiveInfinity));
    }

    [Fact]
    public void Equals_Zero_Zero()
    {
      Assert.True(NumericOperator.Equals(0d, 0d));
    }

    [Fact]
    public void Equals_Zero_Epsilon()
    {
      Assert.True(NumericOperator.Equals(0d, double.Epsilon));
    }

    [Fact]
    public void Epsilon_Zero_Equals()
    {
      Assert.True(NumericOperator.Equals(double.Epsilon, 0d));
    }

    [Fact]
    public void Equals_Zero_1E16()
    {
      Assert.True(NumericOperator.Equals(0d, 1E-16));
    }

    [Fact]
    public void Equals_Zero_2E16_Not()
    {
      Assert.False(NumericOperator.Equals(0d, 2E-16));
    }

    [Fact]
    public void Equals_One_1E16()
    {
      Assert.True(NumericOperator.Equals(1d, 1.0000000000000001));
    }

    [Fact]
    public void Equals_One_2E16_Not()
    {
      Assert.False(NumericOperator.Equals(1d, 1.0000000000000002));
    }

    [Fact]
    public void Power()
    {
      Assert.Equal(1, NumericOperator.Power(2, 0));
      Assert.Equal(2, NumericOperator.Power(2, 1));
      Assert.Equal(4, NumericOperator.Power(2, 2));
      Assert.Equal(8, NumericOperator.Power(2, 3));
      Assert.Equal(16, NumericOperator.Power(2, 4));
      Assert.Equal(32, NumericOperator.Power(2, 5));
      Assert.Equal(64, NumericOperator.Power(2, 6));
      Assert.Equal(128, NumericOperator.Power(2, 7));
      Assert.Equal(256, NumericOperator.Power(2, 8));
    }
  }
}
