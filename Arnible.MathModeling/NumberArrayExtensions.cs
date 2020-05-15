using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class NumberArrayExtensions
  {
    public static NumberArray ToNumberArray(this IEnumerable<Number> numbers)
    {
      return NumberArray.Create(numbers);
    }

    public static NumberArray ToNumberArray(this IEnumerable<double> numbers)
    {
      return numbers.Select(v => (Number)v).ToNumberArray();
    }
  }
}
