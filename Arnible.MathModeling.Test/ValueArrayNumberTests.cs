using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Test
{
  public class ValueArrayNumberTests
  {
    [Fact]
    public static void Zero()
    {
      var zero = ValueArrayNumber.Zero(3);
      AreEqual(3, zero.Length);
      IsTrue(zero.IsZero());
      
      ValueArray<Number> nonZero = new Number[] { 0, 2, 0 };
      IsFalse(nonZero.IsZero());
    }
    
    [Fact]
    public void SplitBySignsTest()
    {
      ValueArray<Number> parameters = new Number[] { -0.1, 0.2, -0.3, 0, 0.1 };
      var (first, second) = parameters.SplitValuesBySign();
      
      AreEqual(first, new Number[] { -0.1, 0, -0.3, 0, 0 });
      AreEqual(second, new Number[] { 0, 0.2, 0, 0, 0.1 });
    }
    
    [Fact]
    public void GetNegativeValues()
    {
      ValueArray<Number> parameters = new Number[] { -0.1, 0.2, -0.3, 0, 0.1 };
      var first = parameters.GetNegativeValues();
      
      AreEqual(first, new Number[] { -0.1, 0, -0.3, 0, 0 });
    }
    
    [Fact]
    public void GetPositiveValues()
    {
      ValueArray<Number> parameters = new Number[] { -0.1, 0.2, -0.3, 0, 0.1 };
      var second = parameters.GetPositiveValues();
      
      AreEqual(second, new Number[] { 0, 0.2, 0, 0, 0.1 });
    }
  }
}