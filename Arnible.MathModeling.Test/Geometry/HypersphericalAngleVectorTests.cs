using System;
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
      Assert.True(v == 0);
      Assert.Equal(1u, v.Length);
      Assert.Equal(0, v[0]);
      Assert.Equal("0", v.ToString());

      Assert.Equal(default, v);
      Assert.Equal(default, new HypersphericalAngleVector());
      Assert.Equal(default, new HypersphericalAngleVector(new Number[0]));      

      Assert.Equal(0, v.GetOrDefault(1));
    }

    [Fact]
    public void Constructor_Single()
    {
      HypersphericalAngleVector v = 2;
      Assert.False(v == 0);
      Assert.True(v == 2);
      Assert.False(v != 2);
      Assert.Equal(2, v[0]);
      Assert.Equal(1u, v.Length);
      Assert.Equal("2", v.ToString());

      Assert.Equal(2, v.GetOrDefault(0));
      Assert.Equal(0, v.GetOrDefault(1));
    }

    [Fact]
    public void Constructor_Explicit()
    {
      HypersphericalAngleVector v = new HypersphericalAngleVector(2, -1, 1);
      Assert.False(v == 0);
      Assert.Equal(v, new Number[] { 2, -1, 1 });
      Assert.Equal(3u, v.Length);
      Assert.Equal("[2 -1 1]", v.ToString());
    }

    [Fact]
    public void NotEqual_Values()
    {
      Assert.NotEqual(new HypersphericalAngleVector(1, 1), new HypersphericalAngleVector(1, -1));
    }

    [Fact]
    public void NotEqual_Dimensions()
    {
      Assert.NotEqual(default, new HypersphericalAngleVector(1));
    }

    [Fact]
    public void Add_NoRound_Positive()
    {
      var a = new HypersphericalAngleVector(π_4, π_4);
      var b = new HypersphericalAngleVector(π_2, π_4);
      Assert.Equal(new HypersphericalAngleVector(3 * π_4, π_2), a + b);
    }

    [Fact]
    public void Add_Superset()
    {
      var a = new HypersphericalAngleVector(π_4);
      var b = new HypersphericalAngleVector(π_2, π_4);
      Assert.Equal(new HypersphericalAngleVector(3 * π_4, π_4), a + b);
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
      Assert.Equal(new HypersphericalAngleVector(3.0 / 4 * π, 3.0 / 8 * π), new[] { a, b }.Average());
    }

    [Theory]
    [InlineData(0, π)]
    [InlineData(π_4, -3 * π_4)]
    [InlineData(π_2, -1 * π_2)]
    [InlineData(π_2 + π_4, -1 * π_4)]
    public void Mirror_Single(double original, double result)
    {
      var a = new HypersphericalAngleVector(original);      
      Assert.Equal(new HypersphericalAngleVector(result), a.Mirror);
    }

    [Theory]
    [InlineData(0, 0, π, 0)]
    [InlineData(π_4, π_4, - 3 * π_4, -1* π_4)]    
    public void Mirror_Two(double original1, double original2, double result1, double result2)
    {
      var a = new HypersphericalAngleVector(original1, original2);
      Assert.Equal(new HypersphericalAngleVector(result1, result2), a.Mirror);
    }
  }
}
