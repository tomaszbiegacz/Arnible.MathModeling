using Arnible.Assertions;
using Arnible.Linq;
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

      IsEqualToExtensions.AssertIsEqualTo(2u, cc.DimensionsCount);
      IsEqualToExtensions.AssertIsEqualTo(2u, cc.Coordinates.Length);
      IsEqualToExtensions.AssertIsEqualTo<double>(3, (double)cc.Coordinates[0]);
      IsEqualToExtensions.AssertIsEqualTo<double>(4, (double)cc.Coordinates[1]);
    }

    [Fact]
    public void Constructor_3d()
    {
      CartesianCoordinate cc = new NumberVector(2, 3, 4);

      IsEqualToExtensions.AssertIsEqualTo(3u, cc.DimensionsCount);
      IsEqualToExtensions.AssertIsEqualTo<double>(2, (double)cc.Coordinates[0]);
      IsEqualToExtensions.AssertIsEqualTo<double>(3, (double)cc.Coordinates[1]);
      IsEqualToExtensions.AssertIsEqualTo<double>(4, (double)cc.Coordinates[2]);
    }

    [Fact]
    public void Equal_Rounding()
    {
      CartesianCoordinate v1 = new NumberVector(1, 1, 0);
      CartesianCoordinate v2 = new NumberVector(1, 1, 8.65956056235496E-17);
      IsEqualToExtensions.AssertIsEqualTo(v1, v2);
    }

    [Fact]
    public void GetDirectionDerivativeRatios_Identity_2()
    {
      NumberVector c = new NumberVector(1, 1);
      var actual = c.GetDirectionDerivativeRatios();

      var expected = HypersphericalAngleVector.GetIdentityVector(2).GetCartesianAxisViewsRatios();
      IsEqualToExtensions.AssertIsEqualTo(expected, actual);
    }
    
    [Fact]
    public void GetDirectionDerivativeRatios_Identity_3()
    {
      NumberVector c = new NumberVector(4, 4, 4);
      var actual = c.GetDirectionDerivativeRatios();

      var expected = HypersphericalAngleVector.GetIdentityVector(3).GetCartesianAxisViewsRatios();
      IsEqualToExtensions.AssertIsEqualTo(expected, actual);
    }
    
    [Fact]
    public void GetDirectionDerivativeRatios_Random()
    {
      ReadOnlyArray<Number> c = new Number[] { 1, 2, -3 };
      var radios = c.GetDirectionDerivativeRatios();
      IsEqualToExtensions.AssertIsEqualTo(3u, radios.Length);
      
      for (ushort i = 0; i < 2; ++i)
      {
        IsLessThanExtensions.AssertIsLessThan(0, radios[i]);
        IsGreaterThanExtensions.AssertIsGreaterThan(1, radios[i]);
      }
      IsLessThanExtensions.AssertIsLessThan(-1, radios[2]);
      IsGreaterThanExtensions.AssertIsGreaterThan(0, radios[2]);
      
      IsEqualToExtensions.AssertIsEqualTo(1, radios.AsList().Select(r => r*r).SumDefensive());
    }
  }
}
