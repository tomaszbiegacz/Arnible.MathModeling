using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Geometry;
using System.Collections.Generic;
using Xunit;

namespace Arnible.MathModeling.Test.Geometry
{
  public class INumberRangeDomainExtensionsTests
  {
    private readonly INumberRangeDomain _domain = new NumberRangeDomain(-1, 1);
    private readonly IEnumerable<HypersphericalAngleQuantified> _directions = HypersphericalAngleQuantified.GetNonLinearDirections(anglesCount: 1, resolution: 2);

    [Fact]
    public void EmptyRatio()
    {
      NumberVector current = new NumberVector(0, 0);
      Assert.Equal(default, _domain.GetValidTranslation(current, _directions.Where(d => d.Angles.Single() == 2).First(), 0));
    }

    [Fact]
    public void PositiveFull()
    {
      NumberVector current = new NumberVector(0, 0);
      Assert.Equal(new NumberTranslationVector(0, 1), _domain.GetValidTranslation(current, _directions.Where(d => d.Angles.Single() == 2).First(), 0.5));
    }

    [Fact]
    public void PositivePartial()
    {
      NumberVector current = new NumberVector(0, 0.5);
      Assert.Equal(new NumberTranslationVector(0, 0.5), _domain.GetValidTranslation(current, _directions.Where(d => d.Angles.Single() == 2).First(), 0.5));
    }

    [Fact]
    public void NegativeFull()
    {
      NumberVector current = new NumberVector(0, 1);
      Assert.Equal(new NumberTranslationVector(0, -1), _domain.GetValidTranslation(current, _directions.Where(d => d.Angles.Single() == 2).First(), 0.5));
    }

    [Fact]
    public void NegativePartial()
    {
      NumberVector current = new NumberVector(-0.5, 1);
      Assert.Equal(new NumberTranslationVector(-0.5, -0.5), _domain.GetValidTranslation(current, _directions.Where(d => d.Angles.Single() == 1).First(), 0.5));
    }
  }
}
