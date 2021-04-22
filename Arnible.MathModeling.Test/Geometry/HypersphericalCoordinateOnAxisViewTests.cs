using Arnible.Assertions;
using Arnible.Linq;
using Arnible.MathModeling.Algebra;
using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;
using static Arnible.MathModeling.xunit.AssertHelpers;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalCoordinateOnAxisViewTests
  {
    [Fact]
    public void ConversationCircle()
    {
      CartesianCoordinate cc = new CartesianCoordinate(1, 2, 3, 4);

      HypersphericalCoordinateOnAxisView view = cc.ToSphericalView();
      EqualExtensions.AssertEqualTo(cc.Coordinates, view.Coordinates);
      EqualExtensions.AssertEqualTo(cc.DimensionsCount, view.DimensionsCount);

      HypersphericalCoordinate hc = view;
      EqualExtensions.AssertEqualTo(view.R, hc.R);
      EqualExtensions.AssertEqualTo(view.Angles, hc.Angles);
      EqualExtensions.AssertEqualTo(view.DimensionsCount, hc.DimensionsCount);

      HypersphericalCoordinateOnAxisView view2 = hc.ToCartesianView();
      EqualExtensions.AssertEqualTo(hc.R, view2.R);
      EqualExtensions.AssertEqualTo(hc.Angles, view2.Angles);
      EqualExtensions.AssertEqualTo(hc.DimensionsCount, view2.DimensionsCount);

      CartesianCoordinate cc2 = view2;
      EqualExtensions.AssertEqualTo(cc.Coordinates, cc2.Coordinates);
      EqualExtensions.AssertEqualTo(cc.DimensionsCount, cc2.DimensionsCount);
    }

    [Fact]
    public void GetRectangularView()
    {
      CartesianCoordinate cc = new CartesianCoordinate(1, 2, 3, 4);
      HypersphericalCoordinateOnAxisView view = cc.ToSphericalView();

      var rcView = view.GetRectangularView(1, 3);
      EqualExtensions.AssertEqualTo(view.R, rcView.R);
      AreExactlyEqual(2, rcView.X);
      AreExactlyEqual(4, rcView.Y);
    }

    [Fact]
    public void GetLineView()
    {
      CartesianCoordinate cc = new CartesianCoordinate(1, 2, 3, 4);
      HypersphericalCoordinateOnAxisView view = cc.ToSphericalView();

      var rcView = view.GetLineView(1);
      EqualExtensions.AssertEqualTo(view.R, rcView.R);
      EqualExtensions.AssertEqualTo(2d, rcView.X);
    }

    [Theory]
    [InlineData(1u)]
    [InlineData(2u)]
    [InlineData(3u)]
    [InlineData(4u)]
    [InlineData(5u)]
    [InlineData(6u)]
    [InlineData(7u)]
    [InlineData(8u)]
    public void GetIdentityVector(uint dimensionsCount)
    {
      NumberVector vector = HypersphericalCoordinateOnAxisView.GetIdentityVector(dimensionsCount);
      EqualExtensions.AssertEqualTo(dimensionsCount, vector.Length);

      IsTrue(vector.AllWithDefault(v => v > 0 && v <= 1));
      NumberVector.Repeat(HypersphericalCoordinateOnAxisView.GetIdentityVectorRatio(dimensionsCount), dimensionsCount).GetInternalEnumerable().AssertSequenceEqual(vector.GetInternalEnumerable().ToArray());      
      EqualExtensions.AssertEqualTo(1d, vector.Select(v => v * v).SumDefensive());
    }
  }
}
