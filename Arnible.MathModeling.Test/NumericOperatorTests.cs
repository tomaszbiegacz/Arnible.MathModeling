using Arnible.Assertions;
using Xunit;

namespace Arnible.MathModeling.Test
{
  public class NumericOperatorTests
  {
    [Fact]
    public void IsValidNumeric_Special()
    {
      double.NaN.IsValidNumeric().AssertIsFalse();
      double.PositiveInfinity.IsValidNumeric().AssertIsFalse();
      double.NegativeInfinity.IsValidNumeric().AssertIsFalse();
    }

    [Fact]
    public void IsValidNumeric_Valid()
    {
      0d.IsValidNumeric().AssertIsTrue();
    }

    [Fact]
    public void Equals_Infinity_Infinity_Not()
    {
      double.PositiveInfinity.NumericEquals(double.PositiveInfinity).AssertIsFalse();
    }    

    [Fact]
    public void Equals_NaN_NaN_Not()
    {
      double.PositiveInfinity.NumericEquals(double.PositiveInfinity).AssertIsFalse();
    }

    [Fact]
    public void Equals_Zero_Zero()
    {
      0d.NumericEquals(0d).AssertIsTrue();
    }

    [Fact]
    public void Equals_Zero_Epsilon()
    {
      0d.NumericEquals(double.Epsilon).AssertIsTrue();
    }

    [Fact]
    public void Epsilon_Zero_Equals()
    {
      double.Epsilon.NumericEquals(0d).AssertIsTrue();
    }

    [Fact]
    public void Equals_Zero_1E16()
    {
      0d.NumericEquals(1E-16).AssertIsTrue();
    }

    [Fact]
    public void Equals_Zero_1E9_Not()
    {
      0d.NumericEquals(1E-9).AssertIsFalse();
    }

    [Fact]
    public void Equals_One_1E16()
    {
      1d.NumericEquals(1.0000000000000001).AssertIsTrue();
    }

    [Fact]
    public void Equals_One_1E9_Not()
    {
      1d.NumericEquals(1.000000001).AssertIsFalse();
    }

    [Fact]
    public void Power()
    {
      2d.ToPower(0).AssertIsEqualTo(1);
      2d.ToPower(1).AssertIsEqualTo(2);
      2d.ToPower(2).AssertIsEqualTo(4);
      2d.ToPower(3).AssertIsEqualTo(8);
      2d.ToPower(4).AssertIsEqualTo(16);
      2d.ToPower(5).AssertIsEqualTo(32);
      2d.ToPower(6).AssertIsEqualTo(64);
      2d.ToPower(7).AssertIsEqualTo(128);
      2d.ToPower(8).AssertIsEqualTo(256);
    }
  }
}
