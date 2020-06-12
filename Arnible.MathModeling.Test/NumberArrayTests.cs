using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Test
{
  public class NumberArrayTests
  {
    [Fact]
    public void Constructor_Default()
    {
      NumberArray v = default;
      AreEqual(0u, v.Length);
      AreEqual("[]", v.ToString());
      IsTrue(v.IsZero);

      AreEqual(default, v);
      AreEqual(default, new NumberArray());
      AreEqual(default, new NumberArray(new Number[0]));
    }

    [Fact]
    public void Constructor_Explicit()
    {
      NumberArray v = new NumberArray(2, 3, 4);
      AreEquals(v, new Number[] { 2, 3, 4 });
      AreEqual(3u, v.Length);
      AreExactlyEqual(3, v[1]);
      AreEqual("[2 3 4]", v.ToString());
    }

    [Fact]
    public void Indexes()
    {
      AreEquals(LinqEnumerable.RangeUint(0, 3), new NumberArray(1, 2, 3).Indexes());
    }
  }
}
