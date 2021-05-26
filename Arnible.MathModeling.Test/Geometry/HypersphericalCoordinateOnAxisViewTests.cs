using Arnible.Assertions;
using Arnible.Linq;
using Arnible.Linq.Algebra;
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
      var cc = new Number[] {1, 2, 3, 4};

      HypersphericalCoordinateOnAxisView view = cc.ToSphericalView();
      view.Coordinates.AssertIsEqualTo(cc);
      view.DimensionsCount.AssertIsEqualTo(cc.Length);

      HypersphericalCoordinate hc = view;
      hc.R.AssertIsEqualTo(view.R);
      hc.Angles.AssertIsEqualTo(view.Angles);
      hc.DimensionsCount.AssertIsEqualTo(view.DimensionsCount);

      HypersphericalCoordinateOnAxisView view2 = hc.ToCartesianView();
      view2.R.AssertIsEqualTo(hc.R);
      view2.Angles.AssertIsEqualTo(hc.Angles);
      view2.DimensionsCount.AssertIsEqualTo(hc.DimensionsCount);

      var cc2 = view2.Coordinates;
      cc2.AssertIsEqualTo(cc);
      cc2.Length.AssertIsEqualTo(cc.Length);
    }

    [Fact]
    public void GetRectangularView()
    {
      var cc = new Number[] { 1, 2, 3, 4 };
      HypersphericalCoordinateOnAxisView view = cc.ToSphericalView();

      var rcView = view.GetRectangularView(1, 3);
      rcView.R.AssertIsEqualTo(view.R);
      rcView.X.AssertIsEqualTo(2);
      rcView.Y.AssertIsEqualTo(4);
    }

    [Fact]
    public void GetLineView()
    {
      var cc = new Number[] { 1, 2, 3, 4 };
      HypersphericalCoordinateOnAxisView view = cc.ToSphericalView();

      var rcView = view.GetLineView(1);
      rcView.R.AssertIsEqualTo(view.R);
      rcView.X.AssertIsEqualTo(2d);
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
    public void GetIdentityVector(ushort dimensionsCount)
    {
      Number[] vector = HypersphericalCoordinateOnAxisView.GetIdentityVector(dimensionsCount);
      vector.Length.AssertIsEqualTo(dimensionsCount);

      Number ratio = HypersphericalCoordinateOnAxisView.GetIdentityVectorRatio(dimensionsCount);
      vector.AssertAll(v => v == ratio);
      vector.Select(v => v * v).SumDefensive().AssertIsEqualTo(1d);
    }
  }
}
