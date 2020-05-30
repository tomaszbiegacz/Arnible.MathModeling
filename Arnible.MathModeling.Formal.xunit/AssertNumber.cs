using System.Collections.Generic;
using Xunit;

namespace Arnible.MathModeling.xunit
{
  public static class AssertNumber
  {
    public static void Equal(Number expected, Number actual)
    {
      Assert.Equal(expected, actual);
    }

    public static void NotEqual(Number expected, Number actual)
    {
      Assert.NotEqual(expected, actual);
    }

    //
    // Formal specific
    //

    public static void EqualDerivatives(PolynomialDivision value, Number term, IDerivative1 actual)
    {
      PolynomialTerm termSingle = (PolynomialTerm)term;
      PolynomialDivision firstDerivative = value.DerivativeBy(termSingle);
      Equal(firstDerivative, actual.First);      
    }

    //
    // Common
    //

    public static void EqualExact(double expected, double actual) => AssertNumberCommon.EqualExact(expected, actual);

    public static void NotEqualExact(double expected, double actual) => AssertNumberCommon.NotEqualExact(expected, actual);


    public static void Equal(IEnumerable<Number> expected, IEnumerable<Number> actual) => AssertNumberCommon.Equal(expected, actual);

    public static void Equal(IEnumerable<int> expected, IEnumerable<Number> actual) => AssertNumberCommon.Equal(expected, actual);

    public static void Equal(IEnumerable<double> expected, IEnumerable<Number> actual) => AssertNumberCommon.Equal(expected, actual);
  }
}
