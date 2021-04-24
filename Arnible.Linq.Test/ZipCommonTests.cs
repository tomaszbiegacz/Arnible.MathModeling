using System;
using System.Collections.Generic;
using Xunit;

namespace Arnible.Linq.Test
{
  public class ZipCommonTests
  {
    [Fact]
    public void ZipCommon_Dictionary()
    {
      var col1 = new Dictionary<char, uint>
      {
        { 'a', 1 }, {'b', 2}, {'c', 3}
      };
      var col2 = new Dictionary<char, uint>
      {
        { 'a', 3 }, {'d', 2}, {'c', 5}
      };

      var result = col1.ZipCommon(col2, Math.Min);

      Assert.Equal(2, result.Count);
      Assert.Equal(1u, result['a']);
      Assert.Equal(3u, result['c']);
    }
  }
}