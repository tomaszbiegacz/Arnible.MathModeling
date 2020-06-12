using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Test
{
  public class LinqSumTests
  {
    [Fact]
    public void Sum_OfThree_Defensive()
    {
      AreEqual(6d, (new[] { 1d, 2d, 3d }).SumDefensive());
    }

    [Fact]
    public void Sum_OfThree_Default()
    {
      AreEqual(6d, (new[] { 1d, 2d, 3d }).SumWithDefault());
    }

    [Fact]
    public void Product_Default()
    {
      AreEqual(0d, LinqEnumerable.Empty<double>().SumWithDefault());
    }
  }
}
