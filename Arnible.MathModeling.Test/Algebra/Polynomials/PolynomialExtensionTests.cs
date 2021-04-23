﻿using Arnible.Assertions;
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

      IsEqualToExtensions.AssertIsEqualTo(3 * x + y + z, (new[] { 4 * x, y - x, z }).SumWithDefault());
    }

    [Fact]
    public void Polynomial_Product_OfThree()
    {
      Polynomial x = 'x';
      Polynomial z = 'z';

      IsEqualToExtensions.AssertIsEqualTo(z * (x * x - 1), (new[] { x - 1, x + 1, z }).ProductWithDefault());
    }
  }
}
