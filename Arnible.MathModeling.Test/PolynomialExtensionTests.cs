using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Test
{
  public class PolynomialExtensionTests
  {
    [Fact]
    public void Polynomial_Sum_OfThree()
    {
      Polynomial x = 'x';
      Polynomial y = 'y';
      Polynomial z = 'z';

      AreEqual(3 * x + y + z, (new[] { 4 * x, y - x, z }).Sum());
    }

    [Fact]
    public void Polynomial_Product_OfThree()
    {
      Polynomial x = 'x';
      Polynomial z = 'z';

      AreEqual(z * (x * x - 1), (new[] { x - 1, x + 1, z }).Product());
    }
  }
}
