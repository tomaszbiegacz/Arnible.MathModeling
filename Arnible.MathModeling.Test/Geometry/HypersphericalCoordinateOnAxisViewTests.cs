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
      AreEqual(cc.Coordinates, view.Coordinates);
      AreEqual(cc.DimensionsCount, view.DimensionsCount);

      HypersphericalCoordinate hc = view;
      AreEqual(view.R, hc.R);
      AreEqual(view.Angles, hc.Angles);
      AreEqual(view.DimensionsCount, hc.DimensionsCount);

      HypersphericalCoordinateOnAxisView view2 = hc.ToCartesianView();
      AreEqual(hc.R, view2.R);
      AreEqual(hc.Angles, view2.Angles);
      AreEqual(hc.DimensionsCount, view2.DimensionsCount);

      CartesianCoordinate cc2 = view2;
      AreEqual(cc.Coordinates, cc2.Coordinates);
      AreEqual(cc.DimensionsCount, cc2.DimensionsCount);
    }

    [Fact]
    public void GetRectangularView()
    {
      CartesianCoordinate cc = new CartesianCoordinate(1, 2, 3, 4);
      HypersphericalCoordinateOnAxisView view = cc.ToSphericalView();

      var rcView = view.GetRectangularView(1, 3);
      AreEqual(view.R, rcView.R);
      AreExactlyEqual(2, rcView.X);
      AreExactlyEqual(4, rcView.Y);
    }

    [Fact]
    public void GetLineView()
    {
      CartesianCoordinate cc = new CartesianCoordinate(1, 2, 3, 4);
      HypersphericalCoordinateOnAxisView view = cc.ToSphericalView();

      var rcView = view.GetLineView(1);
      AreEqual(view.R, rcView.R);
      AreEqual(2d, rcView.X);
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
      AreEqual(dimensionsCount, vector.Length);

      IsTrue(vector.All(v => v > 0 && v <= 1));
      AreEquals(NumberVector.Repeat(HypersphericalCoordinateOnAxisView.GetIdentityVectorRatio(dimensionsCount), dimensionsCount), vector);      
      AreEqual(1d, vector.Select(v => v * v).SumDefensive());
    }
  }
}
