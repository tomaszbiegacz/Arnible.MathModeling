using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public interface IFunction
  {
    double Value(IReadOnlyDictionary<char, double> x);
  }
}
