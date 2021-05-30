using System;
using System.Collections.Generic;
using Arnible.Assertions;
using Arnible.Linq;
using Arnible.Linq.Algebra;
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

      v.AssertIsEqualTo(default);
      new HypersphericalAngleVector().AssertIsEqualTo(default);
    }

    [Fact]
    public void Constructor_Single()
    {
      HypersphericalAngleVector v = new Number[] { 2 };
      v[0].AssertIsEqualTo(2);
      v.Length.AssertIsEqualTo(1);
    }

    [Fact]
    public void Constructor_Explicit()
    {
      HypersphericalAngleVector v = new Number[] {2, -1, 1};
      v.Length.AssertIsEqualTo(3);
    }

    [Fact]
    public void Add_NoRound_Positive()
    {
      HypersphericalAngleVector a = new Number[] {π_4, π_4};
      HypersphericalAngleVector b = new Number[] {π_2, π_4};
      (a + b).AssertIsEqualTo(new Number[] { 3 * π_4, π_2});
    }

    [Fact]
    public void Add_Superset()
    {
      HypersphericalAngleVector a = new Number[] {π_4, 0};
      HypersphericalAngleVector b = new Number[] {π_2, π_4};
      (a + b).AssertIsEqualTo(new Number[] {3 * π_4, π_4});
    }

    [Fact]
    public void Add_NoRound_Negative()
    {
      HypersphericalAngleVector a = new Number[] {-1 * π_4, -1 * π_4};
      HypersphericalAngleVector b = new Number[] {-1 * π_2, -1 * π_4};
      (a + b).AssertIsEqualTo(new Number[] {-3 * π_4, -1 * π_2});
    }

    [Fact]
    public void Add_FirstRound_Positive()
    {
      HypersphericalAngleVector a = new Number[] {3 * π_4, π_4};
      HypersphericalAngleVector b = new Number[] {π_2, π_4};
      (a + b).AssertIsEqualTo(new Number[] {-3 * π_4, π_2});
    }

    [Fact]
    public void Add_FirstRound_Negative()
    {
      HypersphericalAngleVector a = new Number[] {-3 * π_4, π_4};
      HypersphericalAngleVector b = new Number[] {-1 * π_2, π_4};
      (a + b).AssertIsEqualTo(new Number[] {3 * π_4, π_2});
    }

    [Fact]
    public void Add_SecondRound_Positive()
    {
      HypersphericalAngleVector a = new Number[] {π_2, π_4};
      HypersphericalAngleVector b = new Number[] {π_2, π_2};
      (a + b).AssertIsEqualTo(new Number[] {π, -1 * π_4});
    }

    [Fact]
    public void Add_SecondRound_Negative()
    {
      HypersphericalAngleVector a = new Number[] {π_2, -1 * π_4};
      HypersphericalAngleVector b = new Number[] {π_2, -1 * π_2};
      (a + b).AssertIsEqualTo(new Number[] {π, π_4});
    }

    [Fact]
    public void Scale()
    {
      HypersphericalAngleVector a = new Number[] {1, π_4};
      (a * -3).AssertIsEqualTo(new Number[] {-3, π_4});
    }

    [Fact]
    public void Sum_Two()
    {
      HypersphericalAngleVector a = new Number[] {π_4, π_4};
      HypersphericalAngleVector b = new Number[] {π_2, -1 * π_4};
      (a + b).AssertIsEqualTo(new Number[] {3 * π_4, 0});
    }

    [Theory]
    [InlineData(0, π)]
    [InlineData(π_4, -3 * π_4)]
    [InlineData(π_2, -1 * π_2)]
    [InlineData(π_2 + π_4, -1 * π_4)]
    public void Mirror_Single(double original, double result)
    {
      var a = new HypersphericalAngleVector(new Number[] { original });
      a.GetMirrorAngles().AssertIsEqualTo(new HypersphericalAngleVector(new Number[] { result }));
    }

    [Theory]
    [InlineData(0, 0, π, 0)]
    [InlineData(π_4, π_4, -3 * π_4, -1 * π_4)]
    public void Mirror_Two(double original1, double original2, double result1, double result2)
    {
      var a = new HypersphericalAngleVector(new Number[] { original1, original2 });
      a.GetMirrorAngles().AssertIsEqualTo(new HypersphericalAngleVector(new Number[] { result1, result2 }));
    }

    [Fact]
    public void IdentityVector_2()
    {
      var a = HypersphericalAngleVector.GetIdentityVector(2);
      a.Length.AssertIsEqualTo(1);
      a[0].AssertIsEqualTo(π_4);

      Span<Number> radios = stackalloc Number[2];
      a.GetCartesianAxisViewsRatios(in radios);
      radios[0].AssertIsEqualTo(Math.Sqrt(2) / 2);
      radios[1].AssertIsEqualTo(radios[0]);
    }

    private static void IsIdentityRadiosVector(Span<Number> radios)
    {
      for (ushort i = 0; i < radios.Length; ++i)
      {
        radios[i].AssertIsGreaterEqualThan(0);
        radios[i].AssertIsLessEqualThan(1);
      }
      radios.SumDefensive((in Number r) => r*r).AssertIsEqualTo(1);
    }
    
    [Fact]
    public void IdentityVector_3()
    {
      var a = HypersphericalAngleVector.GetIdentityVector(3);
      a.Length.AssertIsEqualTo(2);
      a[0].AssertIsEqualTo(π_4);

      Span<Number> radios = stackalloc Number[3];
      a.GetCartesianAxisViewsRatios(in radios);
      IsIdentityRadiosVector(radios);
    }
    
    [Fact]
    public void IdentityVector_4()
    {
      var a = HypersphericalAngleVector.GetIdentityVector(4);
      a.Length.AssertIsEqualTo(3);
      a[0].AssertIsEqualTo(π_4);

      Span<Number> radios = stackalloc Number[4];
      a.GetCartesianAxisViewsRatios(in radios);
      IsIdentityRadiosVector(radios);
    }
  }
}
