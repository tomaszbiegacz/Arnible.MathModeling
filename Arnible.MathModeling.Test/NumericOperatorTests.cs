using Arnible.Assertions;
using Xunit;

namespace Arnible.MathModeling.Test
{
  public class NumericOperatorTests
  {
    [Fact]
    public void IsValidNumeric_Special()
    {
      ConditionExtensions.AssertIsFalse(double.NaN.IsValidNumeric());
      ConditionExtensions.AssertIsFalse(double.PositiveInfinity.IsValidNumeric());
      ConditionExtensions.AssertIsFalse(double.NegativeInfinity.IsValidNumeric());
    }

    [Fact]
    public void IsValidNumeric_Valid()
    {
      ConditionExtensions.AssertIsTrue(0d.IsValidNumeric());
    }

    [Fact]
    public void Equals_Infinity_Infinity_Not()
    {
      ConditionExtensions.AssertIsFalse(double.PositiveInfinity.NumericEquals(double.PositiveInfinity));
    }    

    [Fact]
    public void Equals_NaN_NaN_Not()
    {
      ConditionExtensions.AssertIsFalse(double.PositiveInfinity.NumericEquals(double.PositiveInfinity));
    }

    [Fact]
    public void Equals_Zero_Zero()
    {
      ConditionExtensions.AssertIsTrue(0d.NumericEquals(0d));
    }

    [Fact]
    public void Equals_Zero_Epsilon()
    {
      ConditionExtensions.AssertIsTrue(0d.NumericEquals(double.Epsilon));
    }

    [Fact]
    public void Epsilon_Zero_Equals()
    {
      ConditionExtensions.AssertIsTrue(double.Epsilon.NumericEquals(0d));
    }

    [Fact]
    public void Equals_Zero_1E16()
    {
      ConditionExtensions.AssertIsTrue(0d.NumericEquals(1E-16));
    }

    [Fact]
    public void Equals_Zero_1E9_Not()
    {
      ConditionExtensions.AssertIsFalse(0d.NumericEquals(1E-9));
    }

    [Fact]
    public void Equals_One_1E16()
    {
      ConditionExtensions.AssertIsTrue(1d.NumericEquals(1.0000000000000001));
    }

    [Fact]
    public void Equals_One_1E9_Not()
    {
      ConditionExtensions.AssertIsFalse(1d.NumericEquals(1.000000001));
    }

    [Fact]
    public void Power()
    {
      EqualExtensions.AssertEqualTo(1, 2d.ToPower(0));
      EqualExtensions.AssertEqualTo(2, 2d.ToPower(1));
      EqualExtensions.AssertEqualTo(4, 2d.ToPower(2));
      EqualExtensions.AssertEqualTo(8, 2d.ToPower(3));
      EqualExtensions.AssertEqualTo(16, 2d.ToPower(4));
      EqualExtensions.AssertEqualTo(32, 2d.ToPower(5));
      EqualExtensions.AssertEqualTo(64, 2d.ToPower(6));
      EqualExtensions.AssertEqualTo(128, 2d.ToPower(7));
      EqualExtensions.AssertEqualTo(256, 2d.ToPower(8));
    }
  }
}
