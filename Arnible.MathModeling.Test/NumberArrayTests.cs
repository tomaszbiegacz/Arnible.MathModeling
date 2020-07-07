using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Test
{
  public class NumberArrayTests
  {
    [Fact]
    public void Constructor_Default()
    {
      ValueArray<Number> v = default;
      AreEqual(0u, v.Length);
      AreEqual("[]", v.ToString());      

      AreEqual(default, v);
      AreEqual(default, new ValueArray<Number>());
      AreEqual(default, new ValueArray<Number>(new Number[0]));      
    }

    [Fact]
    public void Constructor_Explicit()
    {
      ValueArray<Number> v = new Number[] { 2, 3, 4 };
      AreEquals(v, new Number[] { 2, 3, 4 });
      AreEqual(3u, v.Length);
      AreExactlyEqual(3, v[1]);
      AreEqual("[2 3 4]", v.ToString());
    }

    [Fact]
    public void Indexes()
    {
      AreEquals(LinqEnumerable.RangeUint(0, 3), new Number[] { 1, 2, 3 }.ToValueArray().Indexes());
    }

    [Fact]
    public void ToArray_All()
    {
      AreEqual(new ValueArray<Number>(new Number[] { 1, 2, 3 }), new Number[] { 1d, 2d, 3d }.ToValueArray());
    }

    [Fact]
    public void ToArray_WithDefaults()
    {
      AreEqual(new Number[] { 1, 2, 3, 0, 0 }.ToValueArray(), new Number[] { 1d, 2d, 3d }.ToValueArray(5));
    }

    [Fact]
    public void IndexesWhere()
    {
      AreEquals(new[] { 0u, 2u }, new Number[] { 1, 2, 3 }.ToValueArray().IndexesWhere(v => v != 2));
    }

    [Fact]
    public void DistanceSquareTo()
    {
      var a1 = new Number[] { 1, 2, 3 }.ToValueArray();
      var a2 = new Number[] { 2, 1, 1 }.ToValueArray();
      AreEqual(6d, a1.DistanceSquareTo(a2));
    }
  }
}
