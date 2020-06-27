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

    [Fact]
    public void ToArray_All()
    {
      AreEqual(new NumberArray(1, 2, 3), new Number[] { 1d, 2d, 3d }.ToNumberArray());
    }

    [Fact]
    public void ToArray_WithDefaults()
    {
      AreEqual(new NumberArray(1, 2, 3, 0, 0), new Number[] { 1d, 2d, 3d }.ToNumberArray(5));
    }

    [Fact]
    public void IndexesWhere()
    {
      AreEquals(new[] { 0u, 2u }, (new NumberArray(1, 2, 3)).IndexesWhere(v => v != 2));
    }

    [Fact]
    public void DistanceSquareTo()
    {
      var a1 = new NumberArray(1, 2, 3);
      var a2 = new NumberArray(2, 1, 1);
      AreEqual(6d, a1.DistanceSquareTo(a2));
    }
  }
}
