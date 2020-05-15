using Xunit;

namespace Arnible.MathModeling.Test
{
  public class NumberArrayTests
  {
    [Fact]
    public void Constructor_Default()
    {
      NumberArray v = default;
      Assert.Equal(0u, v.Length);
      Assert.Equal("[]", v.ToString());
      Assert.True(v.IsZero);

      Assert.Equal(default, v);
      Assert.Equal(default, new NumberArray());
      Assert.Equal(default, new NumberArray(new Number[0]));
    }

    [Fact]
    public void Constructor_Explicit()
    {
      NumberArray v = new NumberArray(2, 3, 4);
      Assert.Equal(v, new Number[] { 2, 3, 4 });
      Assert.Equal(3u, v.Length);
      Assert.Equal(3, v[1]);
      Assert.Equal("[2 3 4]", v.ToString());
    }

    [Fact]
    public void Indexes()
    {
      Assert.Equal(LinqEnumerable.RangeUint(0, 3), new NumberArray(1, 2, 3).Indexes());
    }
  }
}
