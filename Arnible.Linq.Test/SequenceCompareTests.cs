using Xunit;

namespace Arnible.Linq.Test
{
  public class SequenceCompareTests
  {
    [Fact]
    public void SequenceCompare_Equal()
    {
      Assert.Equal(0, (new[] { 1, 2, 3 }).SequenceCompare(new[] { 1, 2, 3 }));
    }

    [Fact]
    public void SequenceCompare_SecondGreater()
    {
      Assert.Equal(1, (new[] { 1, 2, 3 }).SequenceCompare(new[] { 1, 0, 3 }));
    }

    [Fact]
    public void SequenceCompare_ThirdLower()
    {
      Assert.Equal(-1, (new[] { 1, 2, 3 }).SequenceCompare(new[] { 1, 2, 4 }));
    }
  }
}