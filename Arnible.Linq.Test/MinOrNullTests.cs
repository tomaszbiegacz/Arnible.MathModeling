using Xunit;

namespace Arnible.Linq.Test
{
  public class MinOrNullTests
  {
    [Fact]
    public void Min_OfThree_OrDefault()
    {
      Assert.Equal(2d, (new double?[] { 4d, null, 2d }).MinOrNone());
    }
    
    [Fact]
    public void Min_OfNull_OrDefault()
    {
      Assert.False((new double?[] { null, null, null }).MinOrNone().HasValue);
    }
  }
}