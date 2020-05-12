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
      Assert.False(double.PositiveInfinity.NumericEquals(double.PositiveInfinity));
    }    

    [Fact]
    public void Equals_NaN_NaN_Not()
    {
      Assert.False(double.PositiveInfinity.NumericEquals(double.PositiveInfinity));
    }

    [Fact]
    public void Equals_Zero_Zero()
    {
      Assert.True(0d.NumericEquals(0d));
    }

    [Fact]
    public void Equals_Zero_Epsilon()
    {
      Assert.True(0d.NumericEquals(double.Epsilon));
    }

    [Fact]
    public void Epsilon_Zero_Equals()
    {
      Assert.True(double.Epsilon.NumericEquals(0d));
    }

    [Fact]
    public void Equals_Zero_1E16()
    {
      Assert.True(0d.NumericEquals(1E-16));
    }

    [Fact]
    public void Equals_Zero_1E9_Not()
    {
      Assert.False(0d.NumericEquals(1E-9));
    }

    [Fact]
    public void Equals_One_1E16()
    {
      Assert.True(1d.NumericEquals(1.0000000000000001));
    }

    [Fact]
    public void Equals_One_1E9_Not()
    {
      Assert.False(1d.NumericEquals(1.000000001));
    }

    [Fact]
    public void Power()
    {
      Assert.Equal(1, 2d.ToPower(0));
      Assert.Equal(2, 2d.ToPower(1));
      Assert.Equal(4, 2d.ToPower(2));
      Assert.Equal(8, 2d.ToPower(3));
      Assert.Equal(16, 2d.ToPower(4));
      Assert.Equal(32, 2d.ToPower(5));
      Assert.Equal(64, 2d.ToPower(6));
      Assert.Equal(128, 2d.ToPower(7));
      Assert.Equal(256, 2d.ToPower(8));
    }
  }
}
