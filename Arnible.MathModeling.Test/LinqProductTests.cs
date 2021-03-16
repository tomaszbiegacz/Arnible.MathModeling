using Arnible.Linq;
using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Test
{
  public class LinqProductTests
  {
    [Fact]
    public void Product_OfThree_Defensive()
    {
      AreEqual(24d, (new[] { 4d, 2d, 3d }).ProductDefensive());
    }

    [Fact]
    public void Product_OfThree_Default()
    {
      AreEqual(24d, (new[] { 4d, 2d, 3d }).ProductWithDefault());
    }

    [Fact]
    public void Product_Default()
    {
      AreEqual(1d, LinqArray<double>.Empty.ProductWithDefault());
    }
  }
}
