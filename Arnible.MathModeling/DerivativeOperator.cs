using Arnible.MathModeling.Algebra;
using System;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class DerivativeOperator
  {
    /// <summary>
    /// Polynomial derivative.
    /// </summary>    
    public static IEnumerable<PolynomialTerm> DerivativeBy(this IEnumerable<PolynomialTerm> terms, char name)
    {
      return terms.SelectMany(t => t.DerivativeBy(name));
    }

    private static Number Derivative2Ingredient(IDerivative2[] args, uint pos)
    {
      Number result = args[pos].Second;
      for (uint i = 0; i < pos; ++i)
      {
        result *= args[i].First;
      }
      if (pos + 1 < args.Length)
      {
        Number remainder = 1;
        for (uint i = pos + 1; i < args.Length; ++i)
        {
          remainder *= args[i].First;
        }
        result *= remainder * remainder;
      }
      return result;
    }

    /// <summary>
    /// Simply multiplication of the first derivatives from both enums (composed functions).
    /// </summary>    
    public static IEnumerable<IDerivative1> ForEachElementComposition(
      this IEnumerable<IDerivative1> valueDerrivativeByParameters,
      IEnumerable<IDerivative1> parametersDerivatives)
    {
      return valueDerrivativeByParameters.ZipDefensive(parametersDerivatives, (a, b) => new Derivative1Value(a.First * b.First));
    }

    /// <summary>
    /// Calculates up to second derivatives composition.
    /// </summary>    
    public static IDerivative2 ForComposition(this IEnumerable<IDerivative2> derivatives)
    {
      var args = derivatives.ToArray();
      return new Derivative2Lazy(
        first: args.Select(d => d.First).ProductDefensive(),
        second: () => args.Indexes().Select(pos => Derivative2Ingredient(args, pos)).SumDefensive());
    }

    /// <summary>
    /// Calculates up to second derivatives composition.
    /// </summary>    
    public static IDerivative2 ForComposition(params IDerivative2[] args) => args.ForComposition();

    /// <summary>
    /// Calculates first derivatives composition.
    /// </summary>    
    public static IDerivative1 ForComposition(this IEnumerable<IDerivative1> derivatives)
    {
      return new Derivative1Value(derivatives.Select(d => d.First).ProductDefensive());
    }

    /// <summary>
    /// Calculates first derivatives composition.
    /// </summary>    
    public static IDerivative1 ForComposition(params IDerivative1[] args) => args.ForComposition();

    /// <summary>
    /// Calculate derivative as a sum of (value derivate and other values product) for each value.
    /// </summary>    
    public static IDerivative1 ForProductByParameter(NumberVector productValues, IEnumerable<IDerivative1> valueDerrivativeByParameter)
    {
      if (productValues.Length < 1)
      {
        throw new ArgumentException(nameof(productValues));
      }
      var valueDerivatives = valueDerrivativeByParameter.ToArray();
      if (valueDerivatives.Length != productValues.Length)
      {
        throw new ArgumentException(nameof(valueDerrivativeByParameter));
      }

      Number result = 0;
      for (uint i = 0; i < productValues.Length; ++i)
      {
        Number derivative = valueDerivatives[i].First;
        if (derivative != 0)
        {
          result += productValues.ExcludeAt(i).ProductWithDefault() * derivative;
        }
      }
      return new Derivative1Value(result);
    }
  }
}
