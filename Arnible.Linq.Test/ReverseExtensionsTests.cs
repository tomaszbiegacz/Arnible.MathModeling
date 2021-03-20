using System.Collections.Generic;
using Xunit;

namespace Arnible.Linq.Test
{
  public class ReverseExtensionsTests
  {
    [Fact]
    public void IEnumerable_Reverse()
    {
      IEnumerable<int> items = new[] { 1, 2, 3 };
      items.Reverse().AreEquals(new [] { 3, 2, 1});
    }
    
    [Fact]
    public void IReadOnlyList_Reverse()
    {
      IReadOnlyList<int> items = new[] { 1, 2, 3 };
      items.Reverse().AreEquals(new [] { 3, 2, 1});
    }
    
    [Fact]
    public void IList_Reverse()
    {
      IList<int> items = new[] { 1, 2, 3 };
      items.Reverse().AreEquals(new [] { 3, 2, 1});
    }
  }
}