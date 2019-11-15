using Xunit;

namespace Arnible.MathModeling.Test
{
  public class DerivativeOperatorTests
  {
    [Fact]
    public void Multiply_TwoValues()
    {
      var d1 = new DerivativeValue(2, 3);
      var d2 = new DerivativeValue(5, 7);
      var d = DerivativeOperator.Multiply(d1, d2);
      Assert.Equal(2 * 5, d.First);
      Assert.Equal(3 * 5 * 5 + 2 * 7, d.Second);
    }

    [Fact]
    public void Multiply_ThreeValues()
    {
      var d1 = new DerivativeValue(2, 3);
      var d2 = new DerivativeValue(5, 7);
      var d3 = new DerivativeValue(11, 13);
      var d = DerivativeOperator.Multiply(d1, d2, d3);
      Assert.Equal(2 * 5 * 11, d.First);
      Assert.Equal(3 * 5 * 5 * 11 * 11 + 2 * 7 * 11 * 11 + 2 * 5 * 13, d.Second);
    }
  }
}
