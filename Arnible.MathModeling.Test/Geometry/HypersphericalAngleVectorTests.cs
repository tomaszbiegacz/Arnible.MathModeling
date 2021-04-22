using System;
using Arnible.Assertions;
using Arnible.MathModeling.Algebra;
using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;
using static Arnible.MathModeling.xunit.AssertHelpers;

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
      IsTrue(v == 0);
      AreEqual(1u, v.Length);
      AreExactlyEqual(0, v[0]);
      AreEqual("0", v.ToString());

      EqualExtensions.AssertEqualTo(default, v);
      EqualExtensions.AssertEqualTo(default, new HypersphericalAngleVector());
      EqualExtensions.AssertEqualTo(default, new HypersphericalAngleVector(new Number[0]));

      AreExactlyEqual(0, v.GetOrDefault(1));
    }

    [Fact]
    public void Constructor_Single()
    {
      HypersphericalAngleVector v = 2;
      IsFalse(v == 0);
      IsTrue(v == 2);
      IsFalse(v != 2);
      AreExactlyEqual(2, v[0]);
      EqualExtensions.AssertEqualTo(1u, v.Length);
      EqualExtensions.AssertEqualTo("2", v.ToString());

      AreExactlyEqual(2, v.GetOrDefault(0));
      AreExactlyEqual(0, v.GetOrDefault(1));
    }

    [Fact]
    public void Constructor_Explicit()
    {
      HypersphericalAngleVector v = new HypersphericalAngleVector(2, -1, 1);
      IsFalse(v == 0);
      EqualExtensions.AssertEqualTo(3u, v.Length);
      EqualExtensions.AssertEqualTo("[2 -1 1]", v.ToString());
    }

    [Fact]
    public void NotEqual_Values()
    {
      IsFalse(new HypersphericalAngleVector(1, 1) == new HypersphericalAngleVector(1, -1));
    }

    [Fact]
    public void NotEqual_Dimensions()
    {
      IsFalse(default == new HypersphericalAngleVector(1));
    }

    [Fact]
    public void Add_NoRound_Positive()
    {
      var a = new HypersphericalAngleVector(π_4, π_4);
      var b = new HypersphericalAngleVector(π_2, π_4);
      EqualExtensions.AssertEqualTo(new HypersphericalAngleVector(3 * π_4, π_2), a + b);
    }

    [Fact]
    public void Add_Superset()
    {
      var a = new HypersphericalAngleVector(π_4);
      var b = new HypersphericalAngleVector(π_2, π_4);
      EqualExtensions.AssertEqualTo(new HypersphericalAngleVector(3 * π_4, π_4), a + b);
    }

    [Fact]
    public void Add_NoRound_Negative()
    {
      var a = new HypersphericalAngleVector(-1 * π_4, -1 * π_4);
      var b = new HypersphericalAngleVector(-1 * π_2, -1 * π_4);
      EqualExtensions.AssertEqualTo(new HypersphericalAngleVector(-3 * π_4, -1 * π_2), a + b);
    }

    [Fact]
    public void Add_FirstRound_Positive()
    {
      var a = new HypersphericalAngleVector(3 * π_4, π_4);
      var b = new HypersphericalAngleVector(π_2, π_4);
      EqualExtensions.AssertEqualTo(new HypersphericalAngleVector(-3 * π_4, π_2), a + b);
    }

    [Fact]
    public void Add_FirstRound_Negative()
    {
      var a = new HypersphericalAngleVector(-3 * π_4, π_4);
      var b = new HypersphericalAngleVector(-1 * π_2, π_4);
      EqualExtensions.AssertEqualTo(new HypersphericalAngleVector(3 * π_4, π_2), a + b);
    }

    [Fact]
    public void Add_SecondRound_Positive()
    {
      var a = new HypersphericalAngleVector(π_2, π_4);
      var b = new HypersphericalAngleVector(π_2, π_2);
      EqualExtensions.AssertEqualTo(new HypersphericalAngleVector(π, -1 * π_4), a + b);
    }

    [Fact]
    public void Add_SecondRound_Negative()
    {
      var a = new HypersphericalAngleVector(π_2, -1 * π_4);
      var b = new HypersphericalAngleVector(π_2, -1 * π_2);
      EqualExtensions.AssertEqualTo(new HypersphericalAngleVector(π, π_4), a + b);
    }

    [Fact]
    public void Scale()
    {
      var a = new HypersphericalAngleVector(1, π_4);
      EqualExtensions.AssertEqualTo(new HypersphericalAngleVector(-3, π_4), a * -3);
    }

    [Fact]
    public void Sum_One()
    {
      var a = new HypersphericalAngleVector(π, π_2);
      EqualExtensions.AssertEqualTo(a, new[] { a }.SumDefensive());
    }

    [Fact]
    public void Sum_Two()
    {
      var a = new HypersphericalAngleVector(π_4, π_4);
      var b = new HypersphericalAngleVector(π_2, -1 * π_4);
      EqualExtensions.AssertEqualTo(new HypersphericalAngleVector(3 * π_4, 0), new[] { a, b }.SumDefensive());
    }

    [Fact]
    public void Average_One()
    {
      var a = new HypersphericalAngleVector(π, π_2);
      EqualExtensions.AssertEqualTo(a, new[] { a }.Average());
    }

    [Fact]
    public void Average_Two()
    {
      var a = new HypersphericalAngleVector(π, π_2);
      var b = new HypersphericalAngleVector(π_2, π_4);
      EqualExtensions.AssertEqualTo(new HypersphericalAngleVector(3.0 / 4 * π, 3.0 / 8 * π), new[] { a, b }.Average());
    }

    [Theory]
    [InlineData(0, π)]
    [InlineData(π_4, -3 * π_4)]
    [InlineData(π_2, -1 * π_2)]
    [InlineData(π_2 + π_4, -1 * π_4)]
    public void Mirror_Single(double original, double result)
    {
      var a = new HypersphericalAngleVector(original);
      EqualExtensions.AssertEqualTo(new HypersphericalAngleVector(result), a.Mirror);
    }

    [Theory]
    [InlineData(0, 0, π, 0)]
    [InlineData(π_4, π_4, -3 * π_4, -1 * π_4)]
    public void Mirror_Two(double original1, double original2, double result1, double result2)
    {
      var a = new HypersphericalAngleVector(original1, original2);
      EqualExtensions.AssertEqualTo(new HypersphericalAngleVector(result1, result2), a.Mirror);
    }

    [Fact]
    public void IdentityVector_2()
    {
      var a = HypersphericalAngleVector.GetIdentityVector(2);
      EqualExtensions.AssertEqualTo(1u, a.Length);
      EqualExtensions.AssertEqualTo(π_4, a[0]);

      var radios = a.GetCartesianAxisViewsRatios();
      EqualExtensions.AssertEqualTo(2u, radios.Length);
      EqualExtensions.AssertEqualTo(Math.Sqrt(2) / 2, radios[0]);
      EqualExtensions.AssertEqualTo(radios[0], radios[1]);
    }

    private static void IsIdentityRadiosVector(NumberVector radios)
    {
      for (ushort i = 0; i < radios.Length; ++i)
      {
        IsGreaterEqualThan(0, radios[i]);
        IsLowerEqualThan(1, radios[i]);
      }
      EqualExtensions.AssertEqualTo(1, radios.Select(r => r*r).SumDefensive());
    }
    
    [Fact]
    public void IdentityVector_3()
    {
      var a = HypersphericalAngleVector.GetIdentityVector(3);
      EqualExtensions.AssertEqualTo(2u, a.Length);
      EqualExtensions.AssertEqualTo(π_4, a[0]);

      var radios = a.GetCartesianAxisViewsRatios();
      EqualExtensions.AssertEqualTo(3u, radios.Length);
      IsIdentityRadiosVector(radios);
    }
    
    [Fact]
    public void IdentityVector_4()
    {
      var a = HypersphericalAngleVector.GetIdentityVector(4);
      EqualExtensions.AssertEqualTo(3u, a.Length);
      EqualExtensions.AssertEqualTo(π_4, a[0]);

      var radios = a.GetCartesianAxisViewsRatios();
      EqualExtensions.AssertEqualTo(4u, radios.Length);
      IsIdentityRadiosVector(radios);
    }
  }
}
