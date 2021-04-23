using Arnible.Assertions;
using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class MetricsExtensionsTests
  {
    [Fact]
    public void ManhattanDistance()
    {
      ReadOnlyArray<Number> src = new Number[] {1, 2, 3};
      IsEqualToExtensions.AssertIsEqualTo(5, src.ManhattanDistance(new Number[] { 2, -2, 3 }));
    }
    
    [Fact]
    public void ChebyshevDistance()
    {
      ReadOnlyArray<Number> src = new Number[] {1, 2, 3};
      IsEqualToExtensions.AssertIsEqualTo(4, src.ChebyshevDistance(new Number[] { 2, -2, 3 }));
    }
  }
}