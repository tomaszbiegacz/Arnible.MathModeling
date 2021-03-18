using Xunit;

namespace Arnible.Linq.Test
{
  public class LinqEnumerableTests
  {
    [Fact]
    public void YieldTests()
    {
      int i = 1;
      Assert.True(LinqEnumerable.Yield(i).SequenceEqual(new[] { 1 }));
    }
    
    [Fact]
    public void RangeInt()
    {
      Assert.True(LinqEnumerable.RangeInt(5).SequenceEqual(new[] { 0, 1, 2, 3, 4 }));
    }
    
    [Fact]
    public void RangeUint()
    {
      Assert.True(LinqEnumerable.RangeUint(5).SequenceEqual(new uint[] { 0, 1, 2, 3, 4 }));
    }
    
    [Fact]
    public void RangeUshort()
    {
      Assert.True(LinqEnumerable.RangeUshort(5).SequenceEqual(new ushort[] { 0, 1, 2, 3, 4 }));
    }
  }
}