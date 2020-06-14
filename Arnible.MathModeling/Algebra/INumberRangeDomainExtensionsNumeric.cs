using System;
using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  public static class INumberRangeDomainExtensionsNumeric
  {
    //
    // GetValidTranslation
    //

    private static Number GetValidTranslationRatio(INumberRangeDomain domain, IEnumerable<Number> value, NumberTranslationVector delta)
    {
      return value.Zip(delta, (v, t) => domain.GetValidTranslationRatio(v ?? 0, t ?? 0)).MinDefensive();
    }

    private static NumberTranslationVector GetValidTranslationEnum(INumberRangeDomain domain, IEnumerable<Number> value, NumberTranslationVector delta)
    {
      Number ratio = GetValidTranslationRatio(domain, value, delta);
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

    public static NumberTranslationVector GetValidTranslation(this INumberRangeDomain domain, NumberVector value, NumberTranslationVector delta)
    {
      return GetValidTranslationEnum(domain, value, delta);
    }

    public static NumberTranslationVector GetValidTranslation(this INumberRangeDomain domain, NumberArray value, NumberTranslationVector delta)
    {
      if (delta.Length > value.Length)
      {
        throw new ArgumentException(nameof(value));
      }
      return GetValidTranslationEnum(domain, value, delta);
    }
  }
}
