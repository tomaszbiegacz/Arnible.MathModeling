﻿using Arnible.MathModeling.xunit;
using System;
using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class PolarCoordinatesTests
  {
    const double Sqrt2 = 1.4142135623731;

    [Fact]
    public void Constructor_Default()
    {
      RectangularCoordianate rc = default;
      PolarCoordinate pc = rc.ToPolar();

      AssertNumber.Equal(0, pc.R);
      AssertNumber.Equal(0, pc.Φ);
    }

    [Theory]
    [InlineData(1, 0, 1, 0)]
    [InlineData(0, 1, 1, Math.PI / 2)]
    [InlineData(1, 1, Sqrt2, Math.PI / 4)]
    public void Constructor_Cases(double x, double y, double r, double φ)
    {
      PolarCoordinate pc = (new RectangularCoordianate(x, y)).ToPolar();

      AssertNumber.Equal(r, pc.R);
      AssertNumber.Equal(φ, pc.Φ);
    }
  }
}
