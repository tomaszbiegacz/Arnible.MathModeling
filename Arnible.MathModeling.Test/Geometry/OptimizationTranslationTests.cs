using Arnible.MathModeling.Algebra;
using System;
using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class OptimizationTranslationTests
  {
    [Fact]
    public void Minimum_Absolute()
    {
      Assert.Equal(3, OptimizationTranslation.Minimum(value: 6, new Derivative1Value(-2)));
    }

    [Fact]
    public void Minimum_DirectedPosive()
    {
      Assert.Equal(new NumberTranslationVector(0, 3), OptimizationTranslation.Minimum(value: 6, new HypersphericalAngleVector(Angle.RightAngle), new Derivative1Value(-2)));
    }

    [Fact]
    public void Minimum_DirectedNegative()
    {
      double delta = -3 / Math.Sqrt(2);
      Assert.Equal(new NumberTranslationVector(delta, delta), OptimizationTranslation.Minimum(value: 6, new HypersphericalAngleVector(Angle.RightAngle / 2), new Derivative1Value(2)));
    }
  }
}
