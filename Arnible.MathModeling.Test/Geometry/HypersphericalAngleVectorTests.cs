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
    public void Add_NoRound_Positive()
    {
      var a = new HypersphericalAngleVector(π_4, π_4);
      var b = new HypersphericalAngleVector(π_2, π_4);
      Assert.Equal(new HypersphericalAngleVector(3 * π_4, π_2), a + b);
    }

    [Fact]
    public void Add_NoRound_Negative()
    {
      var a = new HypersphericalAngleVector(-1 * π_4, -1 * π_4);
      var b = new HypersphericalAngleVector(-1 * π_2, -1 * π_4);
      Assert.Equal(new HypersphericalAngleVector(-3 * π_4, -1 * π_2), a + b);
    }

    [Fact]
    public void Add_FirstRound_Positive()
    {
      var a = new HypersphericalAngleVector(3 * π_4, π_4);
      var b = new HypersphericalAngleVector(π_2, π_4);
      Assert.Equal(new HypersphericalAngleVector(-3 * π_4, π_2), a + b);
    }

    [Fact]
    public void Add_FirstRound_Negative()
    {
      var a = new HypersphericalAngleVector(-3 * π_4, π_4);
      var b = new HypersphericalAngleVector(-1 * π_2, π_4);
      Assert.Equal(new HypersphericalAngleVector(3 * π_4, π_2), a + b);
    }

    [Fact]
    public void Add_SecondRound_Positive()
    {
      var a = new HypersphericalAngleVector(π_2, π_4);
      var b = new HypersphericalAngleVector(π_2, π_2);
      Assert.Equal(new HypersphericalAngleVector(π, -1 * π_4), a + b);
    }

    [Fact]
    public void Add_SecondRound_Negative()
    {
      var a = new HypersphericalAngleVector(π_2, -1 * π_4);
      var b = new HypersphericalAngleVector(π_2, -1 * π_2);
      Assert.Equal(new HypersphericalAngleVector(π, π_4), a + b);
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
      var a = new HypersphericalAngleVector(π_4, π_4);
      var b = new HypersphericalAngleVector(π_2, -1 * π_4);
      Assert.Equal(new HypersphericalAngleVector(3 * π_4, 0), new[] { a, b }.Sum());
    }

    [Fact]
    public void Average_One()
    {
      var a = new HypersphericalAngleVector(π, π_2);
      Assert.Equal(a, new[] { a }.Average());
    }

    [Fact]
    public void Average_Two()
    {
      var a = new HypersphericalAngleVector(π, π_2);
      var b = new HypersphericalAngleVector(π_2, π_4);
      Assert.Equal(new HypersphericalAngleVector(3.0/4 * π, 3.0/8 * π), new[] { a, b }.Average());
    }
  }
}
