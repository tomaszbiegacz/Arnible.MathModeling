﻿using System;
using Arnible.Assertions;
using Arnible.MathModeling.Test;
using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class PolarCoordinatesTests
  {
    const double Sqrt2 = 1.4142135623731;

    [Fact]
    public void Constructor_Default()
    {
      RectangularCoordinate rc = default;
      PolarCoordinate pc = rc.ToPolar();

      IsEqualToExtensions.AssertIsEqualTo<double>(0, (double)pc.R);
      IsEqualToExtensions.AssertIsEqualTo<double>(0, (double)pc.Φ);
    }

    [Theory]
    [InlineData(1, 0, 1, 0)]
    [InlineData(0, 1, 1, Math.PI / 2)]
    [InlineData(1, 1, Sqrt2, Math.PI / 4)]
    public void Constructor_Cases(double x, double y, double r, double φ)
    {
      PolarCoordinate pc = (new RectangularCoordinate(x, y)).ToPolar();

      IsEqualToExtensions.AssertIsEqualTo(r, pc.R);
      IsEqualToExtensions.AssertIsEqualTo(φ, pc.Φ);
    }
  }
}
