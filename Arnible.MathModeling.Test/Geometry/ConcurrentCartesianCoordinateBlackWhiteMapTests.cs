using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Geometry.Test
{
  public class ConcurrentCartesianCoordinateBlackWhiteMapTests
  {
    [Fact]
    public void Precision_One()
    {
      var map = new ConcurrentCartesianCoordinateBlackWhiteMap(
        leftBottomMapCorner: new Number[] {0, -1, -2},
        rightTopMapCorner: new Number[] {2, 3, 4},
        precision: 1);
      
      AreEqual(3, map.DimensionsCount);
      AreEqual(0, map.MarkedPointsCount);
      IsFalse(map.IsMarked(new Number[] {0, -1, -2}));
      IsFalse(map.IsMarked(new Number[] { 0, 0, 0 }));
      IsFalse(map.IsMarked(new Number[] { 1, 1, 1 }));
      IsFalse(map.IsMarked(new Number[] { 2, 3, 4 }));
      
      IsTrue(map.MarkPoint(new Number[] { 0, 0, 0 }));
      
      AreEqual(1, map.MarkedPointsCount);
      IsTrue(map.IsMarked(new Number[] {0, -1, -2}));
      IsTrue(map.IsMarked(new Number[] { 0, 0, 0 }));
      IsTrue(map.IsMarked(new Number[] { 1, 1, 1 }));
      IsTrue(map.IsMarked(new Number[] { 2, 3, 4 }));
    }
    
    [Fact]
    public void Precision_Two()
    {
      var map = new ConcurrentCartesianCoordinateBlackWhiteMap(
        leftBottomMapCorner: new Number[] {0, -1, -2},
        rightTopMapCorner: new Number[] {2, 3, 4},
        precision: 2);
      // middle: new Number[] {1, 1, 1}
      
      AreEqual(3, map.DimensionsCount);
      AreEqual(0, map.MarkedPointsCount);
      IsFalse(map.IsMarked(new Number[] {0, -1, -2}));
      IsFalse(map.IsMarked(new Number[] { 0, 0, 0 }));
      IsFalse(map.IsMarked(new Number[] { 1, 1, 1 }));
      IsFalse(map.IsMarked(new Number[] { 2, 3, 4 }));
      
      IsTrue(map.MarkPoint(new Number[] { 0, 0, 0 }));
      
      AreEqual(1, map.MarkedPointsCount);
      IsTrue(map.IsMarked(new Number[] {0, -1, -2}));
      IsTrue(map.IsMarked(new Number[] { 0, 0, 0 }));
      IsFalse(map.IsMarked(new Number[] { 1, 1, 1 }));
      IsFalse(map.IsMarked(new Number[] { 2, 3, 4 }));
    }
    
    [Fact]
    public void Precision_Three()
    {
      var map = new ConcurrentCartesianCoordinateBlackWhiteMap(
        leftBottomMapCorner: new Number[] {0, -1, -2},
        rightTopMapCorner: new Number[] {2, 3, 4},
        precision: 3);

      AreEqual(3, map.DimensionsCount);
      AreEqual(0, map.MarkedPointsCount);
      IsFalse(map.IsMarked(new Number[] {0, -1, -2}));
      IsFalse(map.IsMarked(new Number[] { 0, 0, 0 }));
      IsFalse(map.IsMarked(new Number[] { 1, 1, 1 }));
      IsFalse(map.IsMarked(new Number[] { 2, 3, 4 }));
      
      IsTrue(map.MarkPoint(new Number[] { 0, 0, 0 }));
      
      AreEqual(1, map.MarkedPointsCount);
      IsFalse(map.IsMarked(new Number[] {0, -1, -2}));
      IsTrue(map.IsMarked(new Number[] { 0, 0, 0 }));
      IsTrue(map.IsMarked(new Number[] { 0.1, 0.1, 0.1 }));
      IsFalse(map.IsMarked(new Number[] { 1, 1, 1 }));
      IsFalse(map.IsMarked(new Number[] { 2, 3, 4 }));
      
      IsFalse(map.MarkPoint(new Number[] { 0.1, 0.1, 0.1 }));
    }
  }
}