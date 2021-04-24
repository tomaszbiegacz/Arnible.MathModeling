using Xunit;

namespace Arnible.Linq.Test
{
  public class DistinctTests
  {
    [Fact]
    public void Distinct_Number()
    {
      Assert.True((new [] { 1, 2, 2, 3, 2.0 }).Distinct().SequenceEqual(new double[] { 1, 2, 3 }));
    }
  }
}