using Arnible.MathModeling.Geometry;
using System;
using Xunit;

namespace Arnible.MathModeling.Test.Geometry
{
  public class HypersphericalAngleVectorTests
  {
    const double π_4 = Math.PI / 4;
    const double π_2 = Math.PI / 2;
    const double π = Math.PI;

    [Fact]
    public void AddNoRound()
    {
      var a = new HypersphericalAngleVector(π, π_2);
      var b = new HypersphericalAngleVector(π_2, π_4);
      Assert.Equal(new HypersphericalAngleVector(π + π_2, 3 * π_4), a + b);
    }

    [Fact]
    public void AddFirstRound()
    {
      var a = new HypersphericalAngleVector(π + 3 * π_4, π_2);
      var b = new HypersphericalAngleVector(π_2, π_4);
      Assert.Equal(new HypersphericalAngleVector(π + 3 * π_4, 3 * π_4), a + b);
    }

    [Fact]
    public void AddSecondRound()
    {
      var a = new HypersphericalAngleVector(π, 3* π_4);
      var b = new HypersphericalAngleVector(π_2, π_2);
      Assert.Equal(new HypersphericalAngleVector(π + π_2, 3 * π_4), a + b);
    }

    [Fact]
    public void Sum_One()
    {
      var a = new HypersphericalAngleVector(π, π_2);
      Assert.Equal(a, new[] { a }.Sum());
    }

    [Fact]
    public void Sum_Two()
    {
      var a = new HypersphericalAngleVector(π, π_2);
      var b = new HypersphericalAngleVector(π_2, π_4);
      Assert.Equal(new HypersphericalAngleVector(π + π_2, 3 * π_4), new[] { a, b }.Sum());
    }
  }
}
