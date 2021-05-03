using System.Collections.Generic;
using Xunit;

namespace Arnible.Linq.Test
{
  public class NotNullExtensionsTests
  {
    [Fact]
    public void NotNull_OK()
    {
      IEnumerable<string?> src = new string?[] { null, "test", null, "other"};
      Assert.True(src.NotNull().SequenceEqual(new string[] { "test", "other"}));
    }
    
    [Fact]
    public void NotNone_OK()
    {
      IEnumerable<int?> src = new int?[] { null, 1, null, 2};
      Assert.True(src.NotNone().SequenceEqual(new int[] { 1, 2}));
    }
  }
}