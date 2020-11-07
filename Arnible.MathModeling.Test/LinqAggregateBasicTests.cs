using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Test
{
  public class LinqAggregateBasicTests
  {
    struct Value
    {
      public Value(double v)
      {
        V = v;
      }

      public double V { get; }
    }

    [Fact]
    public void Min_OfThree_OrDefault()
    {
      AreEqual(2d, (new Number?[] { 4d, null, 2d }).MinOrDefault());
    }
    
    [Fact]
    public void Min_OfNull_OrDefault()
    {
      IsNull((new Number?[] { null, null, null }).MinOrDefault());
    }
    
    [Fact]
    public void Min_OfThree_Defensive()
    {
      AreEqual(2d, (new[] { 4d, 2d, 3d }).MinDefensive());
    }

    [Fact]
    public void WithMinimum_OfThree()
    {
      Value r = (new[]
      {
        new Value(4), new Value(2), new Value(3)
      }).WithMinimum(v => v.V);
      AreEqual(2d, r.V);
    }

    [Fact]
    public void WithMinimumAt_OfThree()
    {
      uint r = (new[]
      {
        new Value(4), new Value(6), new Value(3)
      }).WithMinimumAt(v => v.V);
      AreEqual(2u, r);
    }

    [Fact]
    public void Max_OfThree_Defensive()
    {
      AreEqual(4d, (new[] { 4d, 2d, 3d }).MaxDefensive());
    }

    [Fact]
    public void WithMaximum_OfThree()
    {
      Value r = (new[]
      {
        new Value(4), new Value(2), new Value(3)
      }).WithMaximum(v => v.V);
      AreEqual(4d, r.V);
    }

    [Fact]
    public void WithMaximumAt_OfThree()
    {
      uint r = (new[]
      {
        new Value(4), new Value(6), new Value(3)
      }).WithMaximumAt(v => v.V);
      AreEqual(1u, r);
    }

    [Fact]
    public void Median_OfThree_Defensive()
    {
      AreEqual(3d, (new[] { 4d, 2d, 3d }).MedianDefensive());
    }

    [Fact]
    public void Median_OfFive_Defensive()
    {
      AreEqual(3d, (new[] { 1d, 3d, 4d, 2d, 3d }).MedianDefensive());
    }

    [Fact]
    public void Median_OfOne_Defensive()
    {
      AreEqual(2d, (new[] { 2d }).MedianDefensive());
    }
  }
}
