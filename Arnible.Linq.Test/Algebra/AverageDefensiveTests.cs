using Xunit;

namespace Arnible.Linq.Algebra.Test
{
  public class AverageDefensiveTests
  {
    [Fact]
    public void BasicAverage()
    {
      Assert.Equal(1.5, (new double[] { 1d, 2d }).AverageDefensive());
    }
  }
}