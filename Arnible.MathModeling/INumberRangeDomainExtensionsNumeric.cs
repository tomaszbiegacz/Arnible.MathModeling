using System;
using Arnible.Assertions;
using Arnible.Linq.Algebra;
using Arnible.MathModeling.Algebra;

namespace Arnible.MathModeling
{
  public static class INumberRangeDomainExtensionsNumeric
  {
    public static Number GetValidTranslationRatio(
      this INumberRangeDomain domain,
      in ReadOnlySpan<Number> value,
      in ReadOnlySpan<Number> delta)
    {
      value.Length.AssertIsGreaterThan(0);
      value.Length.AssertIsEqualTo(delta.Length);
      
      Number result = domain.GetValidTranslationRatio(in value[0], in delta[0]);
      for(ushort i=1; i<value.Length; ++i)
      {
        Number current = domain.GetValidTranslationRatio(in value[i], in delta[i]);
        if(current < result)
        {
          result = current;
        }
      }
      return result;
    }

    public static Number? GetMaximumValidTranslationRatio(
      this INumberRangeDomain domain,
      in ReadOnlySpan<Number> value,
      in ReadOnlySpan<Number> delta)
    {
      value.Length.AssertIsGreaterThan(0);
      value.Length.AssertIsEqualTo(delta.Length);
      
      Number? result = domain.GetMaximumValidTranslationRatio(in value[0], in delta[0]);
      for(ushort i=1; i<value.Length; ++i)
      {
        Number? current = domain.GetMaximumValidTranslationRatio(in value[i], in delta[i]);
        if(result is null || current < result)
        {
          result = current;
        }
      }
      return result;
    }

    public static void GetValidTranslation(
      this INumberRangeDomain domain,
      in ReadOnlySpan<Number> value,
      in Span<Number> delta)
    {
      Number ratio = GetValidTranslationRatio(domain, in value, delta);
      delta.MultiplySelfBy(ratio);
    }
  }
}
