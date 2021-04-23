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
      IsEqualToExtensions.AssertIsEqualTo(1u, v.Length);
      IsEqualToExtensions.AssertIsEqualTo<double>(0, (double)v[0]);
      IsEqualToExtensions.AssertIsEqualTo("0", v.ToString());

      IsEqualToExtensions.AssertIsEqualTo(default, v);
      IsEqualToExtensions.AssertIsEqualTo(default, new HypersphericalAngleVector());
      IsEqualToExtensions.AssertIsEqualTo(default, new HypersphericalAngleVector(new Number[0]));

      IsEqualToExtensions.AssertIsEqualTo<double>(0, (double)v.GetOrDefault(1));
    }

    [Fact]
    public void Constructor_Single()
    {
      HypersphericalAngleVector v = 2;
      ConditionExtensions.AssertIsFalse(v == 0);
      ConditionExtensions.AssertIsTrue(v == 2);
      ConditionExtensions.AssertIsFalse(v != 2);
      IsEqualToExtensions.AssertIsEqualTo<double>(2, (double)v[0]);
      IsEqualToExtensions.AssertIsEqualTo(1u, v.Length);
      IsEqualToExtensions.AssertIsEqualTo("2", v.ToString());

      IsEqualToExtensions.AssertIsEqualTo<double>(2, (double)v.GetOrDefault(0));
      IsEqualToExtensions.AssertIsEqualTo<double>(0, (double)v.GetOrDefault(1));
    }

    [Fact]
    public void Constructor_Explicit()
    {
      HypersphericalAngleVector v = new HypersphericalAngleVector(2, -1, 1);
      ConditionExtensions.AssertIsFalse(v == 0);
      IsEqualToExtensions.AssertIsEqualTo(3u, v.Length);
      IsEqualToExtensions.AssertIsEqualTo("[2 -1 1]", v.ToString());
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
      IsEqualToExtensions.AssertIsEqualTo(new HypersphericalAngleVector(3 * π_4, π_2), a + b);
    }

    [Fact]
    public void Add_Superset()
    {
      var a = new HypersphericalAngleVector(π_4);
      var b = new HypersphericalAngleVector(π_2, π_4);
      IsEqualToExtensions.AssertIsEqualTo(new HypersphericalAngleVector(3 * π_4, π_4), a + b);
    }

    [Fact]
    public void Add_NoRound_Negative()
    {
      var a = new HypersphericalAngleVector(-1 * π_4, -1 * π_4);
      var b = new HypersphericalAngleVector(-1 * π_2, -1 * π_4);
      IsEqualToExtensions.AssertIsEqualTo(new HypersphericalAngleVector(-3 * π_4, -1 * π_2), a + b);
    }

    [Fact]
    public void Add_FirstRound_Positive()
    {
      var a = new HypersphericalAngleVector(3 * π_4, π_4);
      var b = new HypersphericalAngleVector(π_2, π_4);
      IsEqualToExtensions.AssertIsEqualTo(new HypersphericalAngleVector(-3 * π_4, π_2), a + b);
    }

    [Fact]
    public void Add_FirstRound_Negative()
    {
      var a = new HypersphericalAngleVector(-3 * π_4, π_4);
      var b = new HypersphericalAngleVector(-1 * π_2, π_4);
      IsEqualToExtensions.AssertIsEqualTo(new HypersphericalAngleVector(3 * π_4, π_2), a + b);
    }

    [Fact]
    public void Add_SecondRound_Positive()
    {
      var a = new HypersphericalAngleVector(π_2, π_4);
      var b = new HypersphericalAngleVector(π_2, π_2);
      IsEqualToExtensions.AssertIsEqualTo(new HypersphericalAngleVector(π, -1 * π_4), a + b);
    }

    [Fact]
    public void Add_SecondRound_Negative()
    {
      var a = new HypersphericalAngleVector(π_2, -1 * π_4);
      var b = new HypersphericalAngleVector(π_2, -1 * π_2);
      IsEqualToExtensions.AssertIsEqualTo(new HypersphericalAngleVector(π, π_4), a + b);
    }

    [Fact]
    public void Scale()
    {
      var a = new HypersphericalAngleVector(1, π_4);
      IsEqualToExtensions.AssertIsEqualTo(new HypersphericalAngleVector(-3, π_4), a * -3);
    }

    [Fact]
    public void Sum_One()
    {
      var a = new HypersphericalAngleVector(π, π_2);
      IsEqualToExtensions.AssertIsEqualTo(a, new[] { a }.SumDefensive());
    }

    [Fact]
    public void Sum_Two()
    {
      var a = new HypersphericalAngleVector(π_4, π_4);
      var b = new HypersphericalAngleVector(π_2, -1 * π_4);
      IsEqualToExtensions.AssertIsEqualTo(new HypersphericalAngleVector(3 * π_4, 0), new[] { a, b }.SumDefensive());
    }

    [Fact]
    public void Average_One()
    {
      var a = new HypersphericalAngleVector(π, π_2);
      IsEqualToExtensions.AssertIsEqualTo(a, new[] { a }.Average());
    }

    [Fact]
    public void Average_Two()
    {
      var a = new HypersphericalAngleVector(π, π_2);
      var b = new HypersphericalAngleVector(π_2, π_4);
      IsEqualToExtensions.AssertIsEqualTo(new HypersphericalAngleVector(3.0 / 4 * π, 3.0 / 8 * π), new[] { a, b }.Average());
    }

    [Theory]
    [InlineData(0, π)]
    [InlineData(π_4, -3 * π_4)]
    [InlineData(π_2, -1 * π_2)]
    [InlineData(π_2 + π_4, -1 * π_4)]
    public void Mirror_Single(double original, double result)
    {
      var a = new HypersphericalAngleVector(original);
      IsEqualToExtensions.AssertIsEqualTo(new HypersphericalAngleVector(result), a.Mirror);
    }

    [Theory]
    [InlineData(0, 0, π, 0)]
    [InlineData(π_4, π_4, -3 * π_4, -1 * π_4)]
    public void Mirror_Two(double original1, double original2, double result1, double result2)
    {
      var a = new HypersphericalAngleVector(original1, original2);
      IsEqualToExtensions.AssertIsEqualTo(new HypersphericalAngleVector(result1, result2), a.Mirror);
    }

    [Fact]
    public void IdentityVector_2()
    {
      var a = HypersphericalAngleVector.GetIdentityVector(2);
      IsEqualToExtensions.AssertIsEqualTo(1u, a.Length);
      IsEqualToExtensions.AssertIsEqualTo(π_4, a[0]);

      var radios = a.GetCartesianAxisViewsRatios();
      IsEqualToExtensions.AssertIsEqualTo(2u, radios.Length);
      IsEqualToExtensions.AssertIsEqualTo(Math.Sqrt(2) / 2, radios[0]);
      IsEqualToExtensions.AssertIsEqualTo(radios[0], radios[1]);
    }

    private static void IsIdentityRadiosVector(NumberVector radios)
    {
      for (ushort i = 0; i < radios.Length; ++i)
      {
        IsLessThanExtensions.AssertIsLessEqualThan(0, radios[i]);
        IsGreaterThanExtensions.AssertIsGreaterEqualThan(1, radios[i]);
      }
      IsEqualToExtensions.AssertIsEqualTo(1, radios.Select(r => r*r).SumDefensive());
    }
    
    [Fact]
    public void IdentityVector_3()
    {
      var a = HypersphericalAngleVector.GetIdentityVector(3);
      IsEqualToExtensions.AssertIsEqualTo(2u, a.Length);
      IsEqualToExtensions.AssertIsEqualTo(π_4, a[0]);

      var radios = a.GetCartesianAxisViewsRatios();
      IsEqualToExtensions.AssertIsEqualTo(3u, radios.Length);
      IsIdentityRadiosVector(radios);
    }
    
    [Fact]
    public void IdentityVector_4()
    {
      var a = HypersphericalAngleVector.GetIdentityVector(4);
      IsEqualToExtensions.AssertIsEqualTo(3u, a.Length);
      IsEqualToExtensions.AssertIsEqualTo(π_4, a[0]);

      var radios = a.GetCartesianAxisViewsRatios();
      IsEqualToExtensions.AssertIsEqualTo(4u, radios.Length);
      IsIdentityRadiosVector(radios);
    }
  }
}
