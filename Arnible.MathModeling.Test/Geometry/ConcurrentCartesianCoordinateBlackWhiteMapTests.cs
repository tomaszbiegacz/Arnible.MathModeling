using Arnible.Assertions;
using Xunit;

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
      
      IsEqualToExtensions.AssertIsEqualTo(3, map.DimensionsCount);
      IsEqualToExtensions.AssertIsEqualTo(0, map.MarkedPointsCount);
      ConditionExtensions.AssertIsFalse(map.IsMarked(new Number[] {0, -1, -2}));
      ConditionExtensions.AssertIsFalse(map.IsMarked(new Number[] { 0, 0, 0 }));
      ConditionExtensions.AssertIsFalse(map.IsMarked(new Number[] { 1, 1, 1 }));
      ConditionExtensions.AssertIsFalse(map.IsMarked(new Number[] { 2, 3, 4 }));
      
      ConditionExtensions.AssertIsTrue(map.MarkPoint(new Number[] { 0, 0, 0 }));
      
      IsEqualToExtensions.AssertIsEqualTo(1, map.MarkedPointsCount);
      ConditionExtensions.AssertIsTrue(map.IsMarked(new Number[] {0, -1, -2}));
      ConditionExtensions.AssertIsTrue(map.IsMarked(new Number[] { 0, 0, 0 }));
      ConditionExtensions.AssertIsTrue(map.IsMarked(new Number[] { 1, 1, 1 }));
      ConditionExtensions.AssertIsTrue(map.IsMarked(new Number[] { 2, 3, 4 }));
    }
    
    [Fact]
    public void Precision_Two()
    {
      var map = new ConcurrentCartesianCoordinateBlackWhiteMap(
        leftBottomMapCorner: new Number[] {0, -1, -2},
        rightTopMapCorner: new Number[] {2, 3, 4},
        precision: 2);
      // middle: new Number[] {1, 1, 1}
      
      IsEqualToExtensions.AssertIsEqualTo(3, map.DimensionsCount);
      IsEqualToExtensions.AssertIsEqualTo(0, map.MarkedPointsCount);
      ConditionExtensions.AssertIsFalse(map.IsMarked(new Number[] {0, -1, -2}));
      ConditionExtensions.AssertIsFalse(map.IsMarked(new Number[] { 0, 0, 0 }));
      ConditionExtensions.AssertIsFalse(map.IsMarked(new Number[] { 1, 1, 1 }));
      ConditionExtensions.AssertIsFalse(map.IsMarked(new Number[] { 2, 3, 4 }));
      
      ConditionExtensions.AssertIsTrue(map.MarkPoint(new Number[] { 0, 0, 0 }));
      
      IsEqualToExtensions.AssertIsEqualTo(1, map.MarkedPointsCount);
      ConditionExtensions.AssertIsTrue(map.IsMarked(new Number[] {0, -1, -2}));
      ConditionExtensions.AssertIsTrue(map.IsMarked(new Number[] { 0, 0, 0 }));
      ConditionExtensions.AssertIsFalse(map.IsMarked(new Number[] { 1, 1, 1 }));
      ConditionExtensions.AssertIsFalse(map.IsMarked(new Number[] { 2, 3, 4 }));
    }
    
    [Fact]
    public void Precision_Three()
    {
      var map = new ConcurrentCartesianCoordinateBlackWhiteMap(
        leftBottomMapCorner: new Number[] {0, -1, -2},
        rightTopMapCorner: new Number[] {2, 3, 4},
        precision: 3);

      IsEqualToExtensions.AssertIsEqualTo(3, map.DimensionsCount);
      IsEqualToExtensions.AssertIsEqualTo(0, map.MarkedPointsCount);
      ConditionExtensions.AssertIsFalse(map.IsMarked(new Number[] {0, -1, -2}));
      ConditionExtensions.AssertIsFalse(map.IsMarked(new Number[] { 0, 0, 0 }));
      ConditionExtensions.AssertIsFalse(map.IsMarked(new Number[] { 1, 1, 1 }));
      ConditionExtensions.AssertIsFalse(map.IsMarked(new Number[] { 2, 3, 4 }));
      
      ConditionExtensions.AssertIsTrue(map.MarkPoint(new Number[] { 0, 0, 0 }));
      
      IsEqualToExtensions.AssertIsEqualTo(1, map.MarkedPointsCount);
      ConditionExtensions.AssertIsFalse(map.IsMarked(new Number[] {0, -1, -2}));
      ConditionExtensions.AssertIsTrue(map.IsMarked(new Number[] { 0, 0, 0 }));
      ConditionExtensions.AssertIsTrue(map.IsMarked(new Number[] { 0.1, 0.1, 0.1 }));
      ConditionExtensions.AssertIsFalse(map.IsMarked(new Number[] { 1, 1, 1 }));
      ConditionExtensions.AssertIsFalse(map.IsMarked(new Number[] { 2, 3, 4 }));
      
      ConditionExtensions.AssertIsFalse(map.MarkPoint(new Number[] { 0.1, 0.1, 0.1 }));
    }
  }
}