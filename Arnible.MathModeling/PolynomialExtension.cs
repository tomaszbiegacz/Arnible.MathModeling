using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class PolynomialExtension
  {
    public static Polynomial Product(this IEnumerable<Polynomial> x)
    {
      Polynomial current = 1;
      foreach (Polynomial v in x)
      {
        current *= v;
      }
      return current;
    }

    public static Polynomial Sum(this IEnumerable<Polynomial> x)
    {
      Polynomial current = 0;
      foreach (Polynomial v in x)
      {
        current += v;
      }
      return current;
    }
  }
}
