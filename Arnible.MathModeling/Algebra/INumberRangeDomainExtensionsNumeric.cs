using System;

namespace Arnible.MathModeling.Algebra
{
  public static class INumberRangeDomainExtensionsNumeric
  {
    //
    // GetValidTranslation
    //

    private static Number GetValidTranslationRatio(
      INumberRangeDomain domain,
      in ValueArray<Number> value,
      in NumberTranslationVector delta)
    {
      return value.GetInternalEnumerable().Zip(
        col2: delta.GetInternalEnumerable(), 
        merge: (v, t) => domain.GetValidTranslationRatio(v ?? 0, t ?? 0)).MinDefensive();
    }
    
    private static Number GetValidTranslationRatio(
      INumberRangeDomain domain,
      in NumberVector value,
      in NumberTranslationVector delta)
    {
      return value.GetInternalEnumerable().Zip(
        col2: delta.GetInternalEnumerable(), 
        merge: (v, t) => domain.GetValidTranslationRatio(v ?? 0, t ?? 0)).MinDefensive();
    }

    public static NumberTranslationVector GetValidTranslation(
      this INumberRangeDomain domain,
      in NumberVector value,
      in NumberTranslationVector delta)
    {
      Number ratio = GetValidTranslationRatio(domain, in value, in delta);
      if (ratio == 0)
      {
        return default;
      }
      else if (ratio == 1)
      {
        return delta;
      }
      else
      {
        return ratio * delta;
      }
    }

    public static NumberTranslationVector GetValidTranslation(
      this INumberRangeDomain domain,
      in ValueArray<Number> value,
      in NumberTranslationVector delta)
    {
      if (delta.Length > value.Length)
      {
        throw new ArgumentException(nameof(value));
      }
      
      Number ratio = GetValidTranslationRatio(domain, in value, in delta);
      if (ratio == 0)
      {
        return default;
      }
      else if (ratio == 1)
      {
        return delta;
      }
      else
      {
        return ratio * delta;
      }
    }
  }
}
