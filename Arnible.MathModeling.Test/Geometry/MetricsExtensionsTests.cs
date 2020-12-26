using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Geometry.Test
{
  public class MetricsExtensionsTests
  {
    [Fact]
    public void ManhattanDistance()
    {
      ValueArray<Number> src = new Number[] {1, 2, 3};
      AreEqual(5, src.ManhattanDistance(new Number[] { 2, -2, 3 }));
    }
    
    [Fact]
    public void ChebyshevDistance()
    {
      ValueArray<Number> src = new Number[] {1, 2, 3};
      AreEqual(4, src.ChebyshevDistance(new Number[] { 2, -2, 3 }));
    }
  }
}