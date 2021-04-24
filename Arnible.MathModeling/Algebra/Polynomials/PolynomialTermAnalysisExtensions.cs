using System.Collections.Generic;
using Arnible.Linq;

namespace Arnible.MathModeling.Algebra.Polynomials
{
  public static class PolynomialTermAnalysisExtensions
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
