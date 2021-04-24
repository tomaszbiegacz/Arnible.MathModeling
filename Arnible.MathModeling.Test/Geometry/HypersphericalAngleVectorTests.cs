using System;
using Arnible.Assertions;
using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Test;
using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalAngleVectorTests
  {
    const double π_4 = Math.PI / 4;
    const double π_2 = Math.PI / 2;
    const double π = Math.PI;

    [Fact]
    public void Constructor_Default()
    {
      HypersphericalAngleVector v = default;
      ConditionExtensions.AssertIsTrue(v == 0);
      v.Length.AssertIsEqualTo(1);
      v[0].AssertIsEqualTo(0);
      v.ToString().AssertIsEqualTo("0");

      v.AssertIsEqualTo(default);
      new HypersphericalAngleVector().AssertIsEqualTo(default);
      new HypersphericalAngleVector(new Number[0]).AssertIsEqualTo(default);

      v.GetOrDefault(1).AssertIsEqualTo(0);
    }

    [Fact]
    public void Constructor_Single()
    {
      HypersphericalAngleVector v = 2;
      ConditionExtensions.AssertIsFalse(v == 0);
      ConditionExtensions.AssertIsTrue(v == 2);
      ConditionExtensions.AssertIsFalse(v != 2);
      v[0].AssertIsEqualTo(2);
      v.Length.AssertIsEqualTo(1);
      v.ToString().AssertIsEqualTo("2");

      v.GetOrDefault(0).AssertIsEqualTo(2);
      v.GetOrDefault(1).AssertIsEqualTo(0);
    }

    [Fact]
    public void Constructor_Explicit()
    {
      HypersphericalAngleVector v = new HypersphericalAngleVector(2, -1, 1);
      ConditionExtensions.AssertIsFalse(v == 0);
      v.Length.AssertIsEqualTo(3);
      v.ToString().AssertIsEqualTo("[2 -1 1]");
    }

    [Fact]
    public void NotEqual_Values()
    {
      ConditionExtensions.AssertIsFalse(new HypersphericalAngleVector(1, 1) == new HypersphericalAngleVector(1, -1));
    }

    [Fact]
    public void NotEqual_Dimensions()
    {
      ConditionExtensions.AssertIsFalse(default == new HypersphericalAngleVector(1));
    }

    [Fact]
    public void Add_NoRound_Positive()
    {
      var a = new HypersphericalAngleVector(π_4, π_4);
      var b = new HypersphericalAngleVector(π_2, π_4);
      (a + b).AssertIsEqualTo(new HypersphericalAngleVector(3 * π_4, π_2));
    }

    [Fact]
    public void Add_Superset()
    {
      var a = new HypersphericalAngleVector(π_4);
      var b = new HypersphericalAngleVector(π_2, π_4);
      (a + b).AssertIsEqualTo(new HypersphericalAngleVector(3 * π_4, π_4));
    }

    [Fact]
    public void Add_NoRound_Negative()
    {
      var a = new HypersphericalAngleVector(-1 * π_4, -1 * π_4);
      var b = new HypersphericalAngleVector(-1 * π_2, -1 * π_4);
      (a + b).AssertIsEqualTo(new HypersphericalAngleVector(-3 * π_4, -1 * π_2));
    }

    [Fact]
    public void Add_FirstRound_Positive()
    {
      var a = new HypersphericalAngleVector(3 * π_4, π_4);
      var b = new HypersphericalAngleVector(π_2, π_4);
      (a + b).AssertIsEqualTo(new HypersphericalAngleVector(-3 * π_4, π_2));
    }

    [Fact]
    public void Add_FirstRound_Negative()
    {
      var a = new HypersphericalAngleVector(-3 * π_4, π_4);
      var b = new HypersphericalAngleVector(-1 * π_2, π_4);
      (a + b).AssertIsEqualTo(new HypersphericalAngleVector(3 * π_4, π_2));
    }

    [Fact]
    public void Add_SecondRound_Positive()
    {
      var a = new HypersphericalAngleVector(π_2, π_4);
      var b = new HypersphericalAngleVector(π_2, π_2);
      (a + b).AssertIsEqualTo(new HypersphericalAngleVector(π, -1 * π_4));
    }

    [Fact]
    public void Add_SecondRound_Negative()
    {
      var a = new HypersphericalAngleVector(π_2, -1 * π_4);
      var b = new HypersphericalAngleVector(π_2, -1 * π_2);
      (a + b).AssertIsEqualTo(new HypersphericalAngleVector(π, π_4));
    }

    [Fact]
    public void Scale()
    {
      var a = new HypersphericalAngleVector(1, π_4);
      (a * -3).AssertIsEqualTo(new HypersphericalAngleVector(-3, π_4));
    }

    [Fact]
    public void Sum_One()
    {
      var a = new HypersphericalAngleVector(π, π_2);
      new[] { a }.SumDefensive().AssertIsEqualTo(a);
    }

    [Fact]
    public void Sum_Two()
    {
      var a = new HypersphericalAngleVector(π_4, π_4);
      var b = new HypersphericalAngleVector(π_2, -1 * π_4);
      new[] { a, b }.SumDefensive().AssertIsEqualTo(new HypersphericalAngleVector(3 * π_4, 0));
    }

    [Fact]
    public void Average_One()
    {
      var a = new HypersphericalAngleVector(π, π_2);
      new[] { a }.Average().AssertIsEqualTo(a);
    }

    [Fact]
    public void Average_Two()
    {
      var a = new HypersphericalAngleVector(π, π_2);
      var b = new HypersphericalAngleVector(π_2, π_4);
      new[] { a, b }.Average().AssertIsEqualTo(new HypersphericalAngleVector(3.0 / 4 * π, 3.0 / 8 * π));
    }

    [Theory]
    [InlineData(0, π)]
    [InlineData(π_4, -3 * π_4)]
    [InlineData(π_2, -1 * π_2)]
    [InlineData(π_2 + π_4, -1 * π_4)]
    public void Mirror_Single(double original, double result)
    {
      var a = new HypersphericalAngleVector(original);
      a.Mirror.AssertIsEqualTo(new HypersphericalAngleVector(result));
    }

    [Theory]
    [InlineData(0, 0, π, 0)]
    [InlineData(π_4, π_4, -3 * π_4, -1 * π_4)]
    public void Mirror_Two(double original1, double original2, double result1, double result2)
    {
      var a = new HypersphericalAngleVector(original1, original2);
      a.Mirror.AssertIsEqualTo(new HypersphericalAngleVector(result1, result2));
    }

    [Fact]
    public void IdentityVector_2()
    {
      var a = HypersphericalAngleVector.GetIdentityVector(2);
      a.Length.AssertIsEqualTo(1);
      a[0].AssertIsEqualTo(π_4);

      var radios = a.GetCartesianAxisViewsRatios();
      radios.Length.AssertIsEqualTo(2);
      radios[0].AssertIsEqualTo(Math.Sqrt(2) / 2);
      radios[1].AssertIsEqualTo(radios[0]);
    }

    private static void IsIdentityRadiosVector(NumberVector radios)
    {
      for (ushort i = 0; i < radios.Length; ++i)
      {
        IsLessThanExtensions.AssertIsLessEqualThan(0, radios[i]);
        IsGreaterThanExtensions.AssertIsGreaterEqualThan(1, radios[i]);
      }
      radios.Select(r => r*r).SumDefensive().AssertIsEqualTo(1);
    }
    
    [Fact]
    public void IdentityVector_3()
    {
      var a = HypersphericalAngleVector.GetIdentityVector(3);
      a.Length.AssertIsEqualTo(2);
      a[0].AssertIsEqualTo(π_4);

      var radios = a.GetCartesianAxisViewsRatios();
      radios.Length.AssertIsEqualTo(3);
      IsIdentityRadiosVector(radios);
    }
    
    [Fact]
    public void IdentityVector_4()
    {
      var a = HypersphericalAngleVector.GetIdentityVector(4);
      a.Length.AssertIsEqualTo(3);
      a[0].AssertIsEqualTo(π_4);

      var radios = a.GetCartesianAxisViewsRatios();
      radios.Length.AssertIsEqualTo(4);
      IsIdentityRadiosVector(radios);
    }
  }
}
