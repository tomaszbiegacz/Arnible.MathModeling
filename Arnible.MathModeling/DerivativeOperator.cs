using System;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class DerivativeOperator
  {    
    private static Number Derivative2Ingredient(
      in ValueArray<Derivative2Value> args, 
      in uint pos)
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
    public static IEnumerable<Derivative1Value> ForEachElementComposition(
      this IEnumerable<Derivative1Value> valueDerrivativeByParameters,
      IEnumerable<Derivative1Value> parametersDerivatives)
    {
      return valueDerrivativeByParameters.ZipDefensive(parametersDerivatives, (a, b) => new Derivative1Value(a.First * b.First));
    }

    /// <summary>
    /// Calculates up to second derivatives composition.
    /// </summary>    
    public static Derivative2Value ForComposition(this IEnumerable<Derivative2Value> derivatives)
    {
      var args = derivatives.ToValueArray();
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
    /// Calculate derivative as a sum of (value derivate and other values product) for each value.
    /// </summary>    
    public static Derivative1Value ForProductByParameter(
      in ValueArray<Number> productValues, 
      IEnumerable<Derivative1Value> valueDerrivativeByParameter)
    {
      if (productValues.Length < 1)
      {
        throw new ArgumentException(nameof(productValues));
      }
      var valueDerivatives = valueDerrivativeByParameter.ToValueArray();
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
          result += productValues.GetInternalEnumerable().ExcludeAt(i).ProductWithDefault() * derivative;
        }
      }
      return new Derivative1Value(in result);
    }
  }
}
