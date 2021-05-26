using Arnible.Assertions;
using Arnible.Linq;
using Arnible.Linq.Algebra;
using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Test;
using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class CartesianCoordinatesTests
  {
    [Fact]
    public void Cast_RectangularCoordinates()
    {
      var rc = new RectangularCoordinate(3, 4);
      CartesianCoordinate cc = rc;

      cc.DimensionsCount.AssertIsEqualTo(2);
      cc.Coordinates.Length.AssertIsEqualTo(2);
      cc.Coordinates[0].AssertIsEqualTo(3);
      cc.Coordinates[1].AssertIsEqualTo(4);
    }

    [Fact]
    public void Constructor_3d()
    {
      CartesianCoordinate cc = new Number[] { 2, 3, 4 };

      cc.DimensionsCount.AssertIsEqualTo(3);
      cc.Coordinates[0].AssertIsEqualTo(2);
      cc.Coordinates[1].AssertIsEqualTo(3);
      cc.Coordinates[2].AssertIsEqualTo(4);
    }

    [Fact]
    public void Equal_Rounding()
    {
      CartesianCoordinate v1 = new Number[] {1, 1, 0};
      CartesianCoordinate v2 = new Number[] {1, 1, 8.65956056235496E-17};
      v1.AssertIsEqualTo(v2);
    }

    [Fact]
    public void GetDirectionDerivativeRatios_Identity_2()
    {
      ReadOnlyArray<Number> c = new Number[] { 1, 1 };
      var actual = c.GetDirectionDerivativeRatios();

      ReadOnlyArray<Number> expected = HypersphericalAngleVector.GetIdentityVector(2).GetCartesianAxisViewsRatios();
      expected.AssertIsEqualTo(actual);
    }
    
    [Fact]
    public void GetDirectionDerivativeRatios_Identity_3()
    {
      ReadOnlyArray<Number> c = new Number[] {4, 4, 4};
      var actual = c.GetDirectionDerivativeRatios();

      ReadOnlyArray<Number> expected = HypersphericalAngleVector.GetIdentityVector(3).GetCartesianAxisViewsRatios();
      expected.AssertIsEqualTo(actual);
    }
    
    [Fact]
    public void GetDirectionDerivativeRatios_Random()
    {
      ReadOnlyArray<Number> c = new Number[] { 1, 2, -3 };
      var radios = c.GetDirectionDerivativeRatios();
      3u.AssertIsEqualTo(radios.Length);
      
      for (ushort i = 0; i < 2; ++i)
      {
        radios[i].AssertIsGreaterThan(0);
        radios[i].AssertIsLessThan(1);
      }
      radios[2].AssertIsGreaterThan(-1);
      radios[2].AssertIsLessThan(0);
      
      radios.AsList().Select(r => r*r).SumDefensive().AssertIsEqualTo(1);
    }
  }
}
