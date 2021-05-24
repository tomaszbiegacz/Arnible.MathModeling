using Arnible.Assertions;
using Arnible.Linq.Algebra;
using Xunit;

namespace Arnible.MathModeling.Algebra.Polynomials.Tests
{
  public class PolynomialExtensionTests
  {
    [Fact]
    public void Polynomial_Sum_OfThree()
    {
      Polynomial x = 'x';
      Polynomial y = 'y';
      Polynomial z = 'z';

      (new[] { 4 * x, y - x, z }).SumWithDefault().AssertIsEqualTo(3 * x + y + z);
    }

    [Fact]
    public void Polynomial_Product_OfThree()
    {
      Polynomial x = 'x';
      Polynomial z = 'z';

      (new[] { x - 1, x + 1, z }).ProductWithDefault().AssertIsEqualTo(z * (x * x - 1));
    }
  }
}
