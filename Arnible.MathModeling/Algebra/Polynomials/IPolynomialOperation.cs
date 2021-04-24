using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra.Polynomials
{
  interface IPolynomialOperation
  {
    IEnumerable<char> Variables { get; }

    double Value(IReadOnlyDictionary<char, double> x);
  }
}
