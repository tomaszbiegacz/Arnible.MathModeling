using Arnible.MathModeling.Algebra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.MathModeling
{
  public static class DerivativeOperator
  {
    public static IEnumerable<PolynomialTerm> DerivativeBy(this IEnumerable<PolynomialTerm> terms, char name) => terms.SelectMany(t => t.DerivativeBy(name));

    private static Number Derivative2Ingredient(IDerivative2[] args, int pos)
    {
      Number result = args[pos].Second;
      for (int i = 0; i < pos; ++i)
      {
        result *= args[i].First;
      }
      if (pos + 1 < args.Length)
      {
        Number remainder = 1;
        for (int i = pos + 1; i < args.Length; ++i)
        {
          remainder *= args[i].First;
        }
        result *= remainder * remainder;
      }
      return result;
    }

    public static IEnumerable<IDerivative1> ForEachElementComposition(this IEnumerable<IDerivative1> valueDerrivativeByParameters, IEnumerable<IDerivative1> parametersDerivatives)
    {
      return valueDerrivativeByParameters.SelectMerged(parametersDerivatives, (a, b) => new Derivative1Value(a.First * b.First));      
    }

    public static IDerivative2 ForComposition(this IEnumerable<IDerivative2> derivatives)
    {
      var args = derivatives.ToArray();
      if (args.Length < 1)
      {
        throw new ArgumentException(nameof(args));
      }

      return new Derivative2Lazy(
        first: args.Select(d => d.First).Product(),
        second: () => args.Indexes().Select(pos => Derivative2Ingredient(args, pos)).Sum());
    }

    public static IDerivative2 ForComposition(params IDerivative2[] args) => args.ForComposition();

    public static IDerivative1 ForComposition(this IEnumerable<IDerivative1> derivatives)
    {
      return new Derivative1Value(derivatives.Select(d => d.First).Product());      
    }

    public static IDerivative1 ForComposition(params IDerivative1[] args) => args.ForComposition();

    public static IDerivative1 ForProductByParameter(NumberVector productValues, IEnumerable<IDerivative1> valueDerrivativeByParameter)
    {
      if (productValues.Count < 1)
      {
        throw new ArgumentException(nameof(productValues));
      }
      var valueDerivatives = valueDerrivativeByParameter.ToArray();
      if(valueDerivatives.Length != productValues.Count)
      {
        throw new ArgumentException(nameof(valueDerrivativeByParameter));
      }

      Number result = 0;
      for(uint i=0; i< productValues.Count; ++ i)
      {
        result += productValues.ExcludeAt(i).Product() * valueDerivatives[i].First;
      }
      return new Derivative1Value(result);
    }    
  }
}
