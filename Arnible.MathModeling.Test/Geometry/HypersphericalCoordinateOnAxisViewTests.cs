using Arnible.MathModeling.Geometry;
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
      Assert.Equal(cc.Coordinates, view.Coordinates);
      Assert.Equal(cc.DimensionsCount, view.DimensionsCount);

      HypersphericalCoordinate hc = view;
      Assert.Equal(view.R, hc.R);
      Assert.Equal(view.Angles, hc.Angles);
      Assert.Equal(view.DimensionsCount, hc.DimensionsCount);

      HypersphericalCoordinateOnAxisView view2 = hc.ToCartesianView();
      Assert.Equal(hc.R, view2.R);
      Assert.Equal(hc.Angles, view2.Angles);
      Assert.Equal(hc.DimensionsCount, view2.DimensionsCount);

      CartesianCoordinate cc2 = view2;
      Assert.Equal(cc.Coordinates, cc2.Coordinates);
      Assert.Equal(cc.DimensionsCount, cc2.DimensionsCount);      
    }

    [Fact]
    public void GetRectangularView()
    {
      CartesianCoordinate cc = new CartesianCoordinate(1, 2, 3, 4);
      HypersphericalCoordinateOnAxisView view = cc.ToSphericalView();

      var rcView = view.GetRectangularView(1, 3);
      Assert.Equal(view.R, rcView.R);
      Assert.Equal(2, rcView.X);
      Assert.Equal(4, rcView.Y);
    }
  }
}
