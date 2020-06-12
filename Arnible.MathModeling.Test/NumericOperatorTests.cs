using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Test
{
  public class NumericOperatorTests
  {
    [Fact]
    public void IsValidNumeric_Special()
    {
      IsFalse(double.NaN.IsValidNumeric());
      IsFalse(double.PositiveInfinity.IsValidNumeric());
      IsFalse(double.NegativeInfinity.IsValidNumeric());
    }

    [Fact]
    public void IsValidNumeric_Valid()
    {
      IsTrue(0d.IsValidNumeric());
    }

    [Fact]
    public void Equals_Infinity_Infinity_Not()
    {
      IsFalse(double.PositiveInfinity.NumericEquals(double.PositiveInfinity));
    }    

    [Fact]
    public void Equals_NaN_NaN_Not()
    {
      IsFalse(double.PositiveInfinity.NumericEquals(double.PositiveInfinity));
    }

    [Fact]
    public void Equals_Zero_Zero()
    {
      IsTrue(0d.NumericEquals(0d));
    }

    [Fact]
    public void Equals_Zero_Epsilon()
    {
      IsTrue(0d.NumericEquals(double.Epsilon));
    }

    [Fact]
    public void Epsilon_Zero_Equals()
    {
      IsTrue(double.Epsilon.NumericEquals(0d));
    }

    [Fact]
    public void Equals_Zero_1E16()
    {
      IsTrue(0d.NumericEquals(1E-16));
    }

    [Fact]
    public void Equals_Zero_1E9_Not()
    {
      IsFalse(0d.NumericEquals(1E-9));
    }

    [Fact]
    public void Equals_One_1E16()
    {
      IsTrue(1d.NumericEquals(1.0000000000000001));
    }

    [Fact]
    public void Equals_One_1E9_Not()
    {
      IsFalse(1d.NumericEquals(1.000000001));
    }

    [Fact]
    public void Power()
    {
      AreEqual(1, 2d.ToPower(0));
      AreEqual(2, 2d.ToPower(1));
      AreEqual(4, 2d.ToPower(2));
      AreEqual(8, 2d.ToPower(3));
      AreEqual(16, 2d.ToPower(4));
      AreEqual(32, 2d.ToPower(5));
      AreEqual(64, 2d.ToPower(6));
      AreEqual(128, 2d.ToPower(7));
      AreEqual(256, 2d.ToPower(8));
    }
  }
}
