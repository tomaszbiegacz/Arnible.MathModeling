using System.Collections.Generic;
using Xunit;

namespace Arnible.Linq.Test
{
  public class WhereExtensionsTests
  {
    [Fact]
    public void WhereTests()
    {
      IEnumerable<int> src = new int[] { 1, 2, 3, 4 };
      Assert.True(src.Where(i => i%2 == 0).SequenceEqual(new int[] { 2, 4 }));
    }
  }
}