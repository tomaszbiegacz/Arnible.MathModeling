using System.Collections.Generic;
using Xunit;

namespace Arnible.Linq.Test
{
  public class SelectTests
  {
    [Fact]
    public void Select()
    {
      IEnumerable<int> src = new int[] { 1, 2, 3, 4 };
      Assert.True(src.Select(i => i * 2).SequenceEqual(new int[] { 2, 4, 6, 8 }));
    }
    
    [Fact]
    public void SelectWithIndex()
    {
      IEnumerable<int> src = new int[] { 1, 2, 3, 4 };
      Assert.True(src.Select((i, v) => i + v).SequenceEqual(new long[] { 1, 3, 5, 7 }));
    }
    
    [Fact]
    public void SelectMany()
    {
      IEnumerable<int> src = new int[] { 1, 2 };
      Assert.True(src.SelectMany(i => new int[] { i, i+1 }).SequenceEqual(new int[] { 1, 2, 2, 3 }));
    }
  }
}