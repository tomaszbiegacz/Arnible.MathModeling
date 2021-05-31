﻿using System;
using Arnible.Assertions;
using Xunit;
using Arnible.Linq;
using Arnible.Linq.Algebra;

namespace Arnible.MathModeling.Geometry.Test
{
  public class CoordinatesExtensionTests
  {
    const double Sqrt2 = 1.4142135623731;
    const double Sqrt3 = 1.7320508075689;

    const double one_Sqrt2 = 1 / Sqrt2;

    /// <summary>
    /// 45 degrees
    /// </summary>
    const double π_4 = Math.PI / 4;

    /// <summary>
    /// 60 degres
    /// </summary>
    const double π_3 = Math.PI / 3;

    /// <summary>
    /// 90 degrees
    /// </summary>
    const double π_2 = Math.PI / 2;

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    [InlineData(3, 2)]
    [InlineData(-3, 2)]
    [InlineData(3, -2)]
    public void Cast_HyperspehricalEqualsRectangular(double x, double y)
    {
      var rc = new RectangularCoordinate(x, y);
      Span<Number> cc = new Number[] { x, y };

      Span<Number> buffer = stackalloc Number[1];
      var pc = rc.ToPolar();
      HypersphericalCoordinate sc = cc.ToSpherical(in buffer);

      sc.DimensionsCount.AssertIsEqualTo(2);
      sc.R.AssertIsEqualTo(pc.R);
      sc.Angles.Span.Single().AssertIsEqualTo(pc.Φ);
    }

    [Theory]
    [InlineData(new[] { 1d, 1d }, Sqrt2, new[] { π_4 })]
    [InlineData(new[] { one_Sqrt2, one_Sqrt2, 1 }, Sqrt2, new[] { π_4, π_4 })]
    [InlineData(new[] { Sqrt2, Sqrt2, 2 * Sqrt3 }, 4, new[] { π_4, π_3 })]
    public void Cast_ToHypersphericalView(double[] cartesian, double r, double[] angles)
    {
      Span<Number> cc = cartesian.ToNumberArray();

      Span<Number> buffer = stackalloc Number[cartesian.Length - 1];
      HypersphericalCoordinate sc = cc.ToSpherical(in buffer);
      sc.R.AssertIsEqualTo(r);
      sc.Angles.Span.AssertSequenceEqualsTo(angles);
    }

    [Theory]
    [InlineData(new[] { 0d, 1d }, 1, new[] { π_2 })]
    [InlineData(new[] { 0d, 0d, 1d }, 1, new[] { 0, π_2 })]
    [InlineData(new[] { 0d, 0d, 0d, 1d }, 1, new[] { 0, 0, π_2 })]
    [InlineData(new[] { 0d, 0d, 0d, 0, 1d }, 1, new[] { 0, 0, 0, π_2 })]
    [InlineData(new[] { 0d, 0d, 0d, 0, -1d }, 1, new[] { 0, 0, 0, -1 * π_2 })]
    public void Cast_ToHyperspherical(double[] cartesian, double r, double[] angles)
    {
      Span<Number> cc = cartesian.ToNumberArray();

      Span<Number> buffer = stackalloc Number[cartesian.Length - 1];
      HypersphericalCoordinate sc = cc.ToSpherical(in buffer);
      sc.R.AssertIsEqualTo(r);
      sc.Angles.Span.AssertSequenceEqualsTo(angles);
    }

    [Theory]
    [InlineData(new[] { 1d, 1d }, Sqrt2, new[] { π_4 })]
    [InlineData(new[] { one_Sqrt2, one_Sqrt2, 1 }, Sqrt2, new[] { π_4, π_4 })]
    [InlineData(new[] { Sqrt2, Sqrt2, 2 * Sqrt3 }, 4, new[] { π_4, π_3 })]
    public void Cast_ToCartesian(double[] cartesian, double r, double[] angles)
    {
      var sc = new HypersphericalCoordinate(r, angles.ToNumberArray());

      Span<Number> cc = stackalloc Number[cartesian.Length];
      sc.ToCartesian(in cc);
      cc.AssertSequenceEqualsTo(cartesian);
    }

    [Theory]
    [InlineData(new[] { 1d, 1d }, Sqrt2, new[] { π_4 })]
    [InlineData(new[] { one_Sqrt2, one_Sqrt2, 1 }, Sqrt2, new[] { π_4, π_4 })]
    [InlineData(new[] { Sqrt2, Sqrt2, 2 * Sqrt3 }, 4, new[] { π_4, π_3 })]
    public void AddDimension_ToCartesian(double[] cartesian, double r, double[] angles)
    {
      HypersphericalCoordinate sc = new HypersphericalCoordinate(r, angles.ToNumberArray());
      
      Span<Number> scBuffer = new Number[sc.Angles.Length + 1];
      HypersphericalCoordinate scWithExtraDimension = sc.Clone(in scBuffer);

      Span<Number> ccActual = stackalloc Number[cartesian.Length + 1];
      scWithExtraDimension.ToCartesian(in ccActual);
      ccActual.AssertSequenceEqualsTo(cartesian.Append(0).ToArray());
    }
    
    [Theory]
    [InlineData(new[] { 1d, 1d }, Sqrt2, new[] { π_4 })]
    [InlineData(new[] { one_Sqrt2, one_Sqrt2, 1 }, Sqrt2, new[] { π_4, π_4 })]
    [InlineData(new[] { Sqrt2, Sqrt2, 2 * Sqrt3 }, 4, new[] { π_4, π_3 })]
    public void AddDimension_ToSpherical(double[] cartesian, double r, double[] angles)
    {
      HypersphericalCoordinate sc = new HypersphericalCoordinate(r, angles.ToNumberArray());
      
      Span<Number> scBuffer = new Number[sc.Angles.Length + 1];
      HypersphericalCoordinate scWithExtraDimension = sc.Clone(in scBuffer);
      
      Span<Number> ccWithExtraDimension = stackalloc Number[cartesian.Length + 1];
      ccWithExtraDimension.Clear();
      cartesian.ToNumberArray().CopyTo(ccWithExtraDimension);
      
      Span<Number> scBuffer2 = new Number[sc.Angles.Length + 1];
      HypersphericalCoordinate scActual = ccWithExtraDimension.ToSpherical(in scBuffer);
      
      scActual.AssertIsEqualTo(in scWithExtraDimension);
    }
    
    [Fact]
    public void GetDirectionDerivativeRatios_Identity_2()
    {
      ReadOnlySpan<Number> c = new Number[] { 1, 1 };
      Span<Number> actual = stackalloc Number[2];
      c.GetDirectionDerivativeRatios(in actual);

      Span<Number> buffer = stackalloc Number[1];
      Span<Number> expected = stackalloc Number[2];
      HypersphericalAngleVector.GetIdentityVector(in buffer).GetCartesianAxisViewsRatios(in expected);
      expected.AssertSequenceEqualsTo(actual);
    }
    
    [Fact]
    public void GetDirectionDerivativeRatios_Identity_3()
    {
      Span<Number> c = new Number[] {4, 4, 4};
      Span<Number> actual = stackalloc Number[3];
      c.GetDirectionDerivativeRatios(in actual);

      Span<Number> buffer = stackalloc Number[2];
      Span<Number> expected = stackalloc Number[3];
      HypersphericalAngleVector.GetIdentityVector(in buffer).GetCartesianAxisViewsRatios(in expected);
      expected.AssertSequenceEqualsTo(actual);
    }
    
    [Fact]
    public void GetDirectionDerivativeRatios_Random()
    {
      Span<Number> c = new Number[] { 1, 2, -3 };
      Span<Number> radios = stackalloc Number[3];
      c.GetDirectionDerivativeRatios(in radios);
      3.AssertIsEqualTo(radios.Length);
      
      for (ushort i = 0; i < 2; ++i)
      {
        radios[i].AssertIsGreaterThan(0);
        radios[i].AssertIsLessThan(1);
      }
      radios[2].AssertIsGreaterThan(-1);
      radios[2].AssertIsLessThan(0);
      
      radios.SumDefensive((in Number r) => r*r).AssertIsEqualTo(1);
    }
  }
}
