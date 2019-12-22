using System;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.MathModeling
{
  public static class DerivativeOperator
  {
    private static double Derivative2Ingredient(IDerivative2[] args, int pos)
    {
      double result = args[pos].Second;
      for (int i = 0; i < pos; ++i)
      {
        result *= args[i].First;
      }
      if (pos + 1 < args.Length)
      {
        double remainder = 1;
        for (int i = pos + 1; i < args.Length; ++i)
        {
          remainder *= args[i].First;
        }
        result *= remainder * remainder;
      }
      return result;
    }

    public static IDerivative2 Multiply(params IDerivative2[] args)
    {
      // keep an ICollection/array here due to two levels of iteration
      if (args?.Length < 1)
      {
        throw new ArgumentException(nameof(args));
      }

      return new Derivative2Lazy(
        first: args.Select(d => d.First).Product(),
        second: () => args.Indexes().Select(pos => Derivative2Ingredient(args, pos)).Sum());
    }

    public static IEnumerable<PolynomialTerm> DerivativeBy(this IEnumerable<PolynomialTerm> terms, char name) => terms.SelectMany(t => t.DerivativeBy(name));
  }
}
