using System;
using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  public static class INumberRangeDomainExtensions
  {
    public static IEnumerable<Number> ValidateEnum(INumberRangeDomain domain, IEnumerable<Number> value)
    {
      return value.Select(v => domain.Validate(v));
    }

    public static NumberArray Validate(this INumberRangeDomain domain, NumberArray value)
    {
      return ValidateEnum(domain, value).ToNumberArray();
    }

    public static NumberVector Translate(this INumberRangeDomain domain, NumberVector value, NumberTranslationVector delta)
    {
      return value.Zip(delta, (v, t) => domain.Translate(v ?? 0, t ?? 0)).ToVector();
    }

    public static bool IsValidTranslation(this INumberRangeDomain domain, NumberVector value, NumberTranslationVector delta)
    {
      return value.Zip(delta, (v, t) => domain.IsValidTranslation(v ?? 0, t ?? 0)).All();
    }

    public static NumberArray Translate(this INumberRangeDomain domain, NumberArray value, NumberTranslationVector delta)
    {
      if (delta.Length > value.Length)
      {
        throw new ArgumentException(nameof(delta));
      }

      return value.Zip(delta, (v, t) => domain.Translate(v.Value, t ?? 0)).ToNumberArray();
    }

    public static bool IsValidTranslation(this INumberRangeDomain domain, NumberArray value, NumberTranslationVector delta)
    {
      if(delta.Length > value.Length)
      {
        return false;
      }
      else
      {
        return value.Zip(delta, (v, t) => domain.IsValidTranslation(v.Value, t ?? 0)).All();
      }
    }
  }
}
