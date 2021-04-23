using Arnible.Assertions;
using Arnible.Linq;
using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Test;
using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalCoordinateOnAxisViewTests
  {
    [Fact]
    public void ConversationCircle()
    {
      CartesianCoordinate cc = new CartesianCoordinate(1, 2, 3, 4);

      HypersphericalCoordinateOnAxisView view = cc.ToSphericalView();
      IsEqualToExtensions.AssertIsEqualTo(cc.Coordinates, view.Coordinates);
      IsEqualToExtensions.AssertIsEqualTo(cc.DimensionsCount, view.DimensionsCount);

      HypersphericalCoordinate hc = view;
      IsEqualToExtensions.AssertIsEqualTo(view.R, hc.R);
      IsEqualToExtensions.AssertIsEqualTo(view.Angles, hc.Angles);
      IsEqualToExtensions.AssertIsEqualTo(view.DimensionsCount, hc.DimensionsCount);

      HypersphericalCoordinateOnAxisView view2 = hc.ToCartesianView();
      IsEqualToExtensions.AssertIsEqualTo(hc.R, view2.R);
      IsEqualToExtensions.AssertIsEqualTo(hc.Angles, view2.Angles);
      IsEqualToExtensions.AssertIsEqualTo(hc.DimensionsCount, view2.DimensionsCount);

      CartesianCoordinate cc2 = view2;
      IsEqualToExtensions.AssertIsEqualTo(cc.Coordinates, cc2.Coordinates);
      IsEqualToExtensions.AssertIsEqualTo(cc.DimensionsCount, cc2.DimensionsCount);
    }

    [Fact]
    public void GetRectangularView()
    {
      CartesianCoordinate cc = new CartesianCoordinate(1, 2, 3, 4);
      HypersphericalCoordinateOnAxisView view = cc.ToSphericalView();

      var rcView = view.GetRectangularView(1, 3);
      IsEqualToExtensions.AssertIsEqualTo(view.R, rcView.R);
      IsEqualToExtensions.AssertIsEqualTo<double>(2, (double)rcView.X);
      IsEqualToExtensions.AssertIsEqualTo<double>(4, (double)rcView.Y);
    }

    [Fact]
    public void GetLineView()
    {
      CartesianCoordinate cc = new CartesianCoordinate(1, 2, 3, 4);
      HypersphericalCoordinateOnAxisView view = cc.ToSphericalView();

      var rcView = view.GetLineView(1);
      IsEqualToExtensions.AssertIsEqualTo(view.R, rcView.R);
      IsEqualToExtensions.AssertIsEqualTo(2d, rcView.X);
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
      IsEqualToExtensions.AssertIsEqualTo(dimensionsCount, vector.Length);

      ConditionExtensions.AssertIsTrue(vector.AllWithDefault(v => v > 0 && v <= 1));
      NumberVector.Repeat(HypersphericalCoordinateOnAxisView.GetIdentityVectorRatio(dimensionsCount), dimensionsCount).GetInternalEnumerable().AssertSequenceEqualsTo(vector.GetInternalEnumerable().ToArray());      
      IsEqualToExtensions.AssertIsEqualTo(1d, vector.Select(v => v * v).SumDefensive());
    }
  }
}
