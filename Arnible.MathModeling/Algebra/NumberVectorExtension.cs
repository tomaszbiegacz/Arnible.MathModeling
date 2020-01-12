using System.Collections.Generic;
using System.Linq;

namespace Arnible.MathModeling.Algebra
{
  public static class NumberVectorExtension
  {
    public static NumberVector ToVector(this IEnumerable<Number> numbers)
    {
      return new NumberVector(numbers);
    }

    public static NumberVector ToVector(this IEnumerable<double> numbers)
    {
      return new NumberVector(numbers.Select(v => (Number)v));
    }
  }
}
