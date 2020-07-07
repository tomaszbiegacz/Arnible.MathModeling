using System;
using System.Collections.Generic;
using System.Text;

namespace Arnible.MathModeling.Polynomials
{
  public static class PolynomialTermExtensions
  {
    /// <summary>
    /// Polynomial derivative.
    /// </summary>    
    public static IEnumerable<PolynomialTerm> DerivativeBy(this IEnumerable<PolynomialTerm> terms, char name)
    {
      return terms.SelectMany(t => t.DerivativeBy(name));
    }
  }
}
