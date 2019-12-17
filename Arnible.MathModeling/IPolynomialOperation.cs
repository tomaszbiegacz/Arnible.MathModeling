using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public interface IPolynomialOperation
  {
    IEnumerable<char> Variables { get; }

    double Value(IReadOnlyDictionary<char, double> x);
  }
}
