using System;
using System.Collections.Generic;
using Arnible.Linq;
using Arnible.MathModeling.Algebra;

namespace Arnible.MathModeling.Analysis
{
  public static class DerivativeOperator
  {    
    private static Number Derivative2Ingredient(
      IReadOnlyList<Derivative2Value> args, 
      ushort pos)
    {
      Number result = args[pos].Second;
      for (ushort i = 0; i < pos; ++i)
      {
        result *= args[i].First;
      }
      if (pos + 1 < args.Count)
      {
        Number remainder = 1;
        for (ushort i = (ushort)(pos + 1); i < args.Count; ++i)
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
    public static IEnumerable<Derivative1Value> ForEachElementComposition(
      this IEnumerable<Derivative1Value> valueDerivativeByParameters,
      IEnumerable<Derivative1Value> parametersDerivatives)
    {
      return valueDerivativeByParameters
          .ZipDefensive(parametersDerivatives, (a, b) => new Derivative1Value(a.First * b.First));
    }

    /// <summary>
    /// Calculates up to second derivatives composition.
    /// </summary>    
    public static Derivative2Value ForComposition(this IReadOnlyList<Derivative2Value> args)
    {
      return new Derivative2Value(
        first: args.Select(d => d.First).ProductDefensive(),
        second: args.Indexes().Select(pos => Derivative2Ingredient(args, pos)).SumDefensive());
    }
    
    /// <summary>
    /// Calculates first derivatives composition.
    /// </summary>    
    public static Derivative1Value ForComposition(this IEnumerable<Derivative1Value> derivatives)
    {
      return new Derivative1Value(derivatives.Select(d => d.First).ProductDefensive());
    }

    /// <summary>
    /// Calculates first derivatives composition.
    /// </summary>    
    public static Derivative1Value ForComposition(params Derivative1Value[] args) => args.ForComposition();

    /// <summary>
    /// Calculate derivative as a sum of (value derivative and other values product) for each value.
    /// </summary>    
    public static Derivative1Value ForProductByParameter(
      IReadOnlyList<Number> productValues, 
      IReadOnlyList<Derivative1Value> valueDerivativeByParameter)
    {
      if (productValues.Count < 1)
      {
        throw new ArgumentException(nameof(productValues));
      }
      if (valueDerivativeByParameter.Count != productValues.Count)
      {
        throw new ArgumentException(nameof(valueDerivativeByParameter));
      }

      Number result = 0;
      for (ushort i = 0; i < productValues.Count; ++i)
      {
        Number derivative = valueDerivativeByParameter[i].First;
        if (derivative != 0)
        {
          result += productValues.ExcludeAt(i).ProductWithDefault() * derivative;
        }
      }
      return new Derivative1Value(in result);
    }
  }
}
