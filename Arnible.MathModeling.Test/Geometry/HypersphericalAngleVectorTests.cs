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
      v.IsZero().AssertIsTrue();
    }

    [Fact]
    public void Constructor_Single()
    {
      HypersphericalAngleVector v = new Number[] { 2 };
      v.Span[0].AssertIsEqualTo(2);
      v.Length.AssertIsEqualTo(1);
      v.IsOrthogonal().AssertIsTrue();
      v.IsZero().AssertIsFalse();
    }

    [Fact]
    public void Constructor_Explicit()
    {
      Span<Number> vs = new Number[] {2, -1, 1};
      HypersphericalAngleVector v = vs;
      v.Length.AssertIsEqualTo(3);
      v.IsOrthogonal().AssertIsFalse();
      
      Span<Number> buffer = new Number[3];
      HypersphericalAngleVector v2 = v.Clone(in buffer);
      
      v.AssertIsEqualTo(in v2);
    }
    
    [Fact]
    public void Orthogonal()
    {
      Span<Number> buffer = new Number[3];
      HypersphericalAngleVector v2 = HypersphericalAngleVector.CreateOrthogonalDirection(1, 0.6, in buffer);
      v2.Span[1].AssertIsEqualTo(0.6);
      
      v2.IsOrthogonal().AssertIsTrue();
    }
    
    [Fact]
    public void Clone()
    {
      Span<Number> buffer1 = new Number[3];
      buffer1.Fill(1);
      
      Span<Number> buffer2 = new Number[6];
      buffer2.Fill(2);
      
      Span<Number> buffer3 = new Number[6];
      buffer3.Fill(3);
      
      HypersphericalAngleVector v1 = HypersphericalAngleVector.CreateOrthogonalDirection(1, 0.7, in buffer1);
      HypersphericalAngleVector v2 = v1.Clone(in buffer2);
      
      HypersphericalAngleVector v3 = HypersphericalAngleVector.CreateOrthogonalDirection(1, 0.7, in buffer3);
      v3.AssertIsEqualTo(v2);
    }

    [Fact]
    public void Add_NoRound_Positive()
    {
      HypersphericalAngleVector a = new Number[] {π_4, π_4};
      HypersphericalAngleVector b = new Number[] {π_2, π_4};
      a.AddSelf(in b);
      a.AssertIsEqualTo(new Number[] { 3 * π_4, π_2});
    }

    [Fact]
    public void Add_Superset()
    {
      HypersphericalAngleVector a = new Number[] {π_4, 0};
      HypersphericalAngleVector b = new Number[] {π_2, π_4};
      a.AddSelf(in b);
      a.AssertIsEqualTo(new Number[] {3 * π_4, π_4});
    }

    [Fact]
    public void Add_NoRound_Negative()
    {
      HypersphericalAngleVector a = new Number[] {-1 * π_4, -1 * π_4};
      HypersphericalAngleVector b = new Number[] {-1 * π_2, -1 * π_4};
      a.AddSelf(in b);
      a.AssertIsEqualTo(new Number[] {-3 * π_4, -1 * π_2});
    }

    [Fact]
    public void Add_FirstRound_Positive()
    {
      HypersphericalAngleVector a = new Number[] {3 * π_4, π_4};
      HypersphericalAngleVector b = new Number[] {π_2, π_4};
      a.AddSelf(in b);
      a.AssertIsEqualTo(new Number[] {-3 * π_4, π_2});
    }

    [Fact]
    public void Add_FirstRound_Negative()
    {
      HypersphericalAngleVector a = new Number[] {-3 * π_4, π_4};
      HypersphericalAngleVector b = new Number[] {-1 * π_2, π_4};
      a.AddSelf(in b);
      a.AssertIsEqualTo(new Number[] {3 * π_4, π_2});
    }

    [Fact]
    public void Add_SecondRound_Positive()
    {
      HypersphericalAngleVector a = new Number[] {π_2, π_4};
      HypersphericalAngleVector b = new Number[] {π_2, π_2};
      a.AddSelf(in b);
      a.AssertIsEqualTo(new Number[] {π, -1 * π_4});
    }

    [Fact]
    public void Add_SecondRound_Negative()
    {
      HypersphericalAngleVector a = new Number[] {π_2, -1 * π_4};
      HypersphericalAngleVector b = new Number[] {π_2, -1 * π_2};
      a.AddSelf(in b);
      a.AssertIsEqualTo(new Number[] {π, π_4});
    }

    [Fact]
    public void Scale()
    {
      HypersphericalAngleVector a = new Number[] {1, π_4};
      a.ScaleSelf(-3);
      a.AssertIsEqualTo(new Number[] {-3, π_4});
    }

    [Fact]
    public void Sum_Two()
    {
      HypersphericalAngleVector a = new Number[] {π_4, π_4};
      HypersphericalAngleVector b = new Number[] {π_2, -1 * π_4};
      a.AddSelf(in b);
      a.AssertIsEqualTo(new Number[] {3 * π_4, 0});
    }

    [Theory]
    [InlineData(0, π)]
    [InlineData(π_4, -3 * π_4)]
    [InlineData(π_2, -1 * π_2)]
    [InlineData(π_2 + π_4, -1 * π_4)]
    public void Mirror_Single(double original, double result)
    {
      Span<Number> buffer = new Number[1];
      var a = new HypersphericalAngleVector(new Number[] { original });
      a.GetMirrorAngles(in buffer).AssertIsEqualTo(new HypersphericalAngleVector(new Number[] { result }));
    }

    [Theory]
    [InlineData(0, 0, π, 0)]
    [InlineData(π_4, π_4, -3 * π_4, -1 * π_4)]
    public void Mirror_Two(double original1, double original2, double result1, double result2)
    {
      Span<Number> buffer = new Number[2];
      var a = new HypersphericalAngleVector(new Number[] { original1, original2 });
      a.GetMirrorAngles(in buffer).AssertIsEqualTo(new HypersphericalAngleVector(new Number[] { result1, result2 }));
    }

    [Fact]
    public void IdentityVector_2()
    {
      Span<Number> buffer = new Number[1];
      var a = HypersphericalAngleVector.GetIdentityVector(in buffer);
      a.Span[0].AssertIsEqualTo(π_4);

      Span<Number> radios = new Number[2];
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
      Span<Number> buffer = new Number[2];
      var a = HypersphericalAngleVector.GetIdentityVector(in buffer);
      a.Span[0].AssertIsEqualTo(π_4);

      Span<Number> radios = new Number[3];
      a.GetCartesianAxisViewsRatios(in radios);
      IsIdentityRadiosVector(radios);
    }
    
    [Fact]
    public void IdentityVector_4()
    {
      Span<Number> buffer = new Number[3];
      var a = HypersphericalAngleVector.GetIdentityVector(in buffer);
      a.Span[0].AssertIsEqualTo(π_4);

      Span<Number> radios = new Number[4];
      a.GetCartesianAxisViewsRatios(in radios);
      IsIdentityRadiosVector(radios);
    }
  }
}
