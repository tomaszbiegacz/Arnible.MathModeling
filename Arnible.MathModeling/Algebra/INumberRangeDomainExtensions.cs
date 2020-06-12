using System;
using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  public static class INumberRangeDomainExtensions
  {
    //
    // IsValidTranslation
    //
    
    public static bool IsValidTranslation(this INumberRangeDomain domain, Number value, Number delta)
    {
      domain.Validate(value);
      return domain.IsValid(value + delta);
    }

    public static bool IsValidTranslation(this INumberRangeDomain domain, NumberVector value, NumberTranslationVector delta)
    {
      return value.Zip(delta, (v, t) => domain.IsValidTranslation(v ?? 0, t ?? 0)).All();
    }

    public static bool IsValidTranslation(this INumberRangeDomain domain, NumberArray value, NumberTranslationVector delta)
    {
      if (delta.Length > value.Length)
      {
        return false;
      }
      else
      {
        return value.Zip(delta, (v, t) => domain.IsValidTranslation(v.Value, t ?? 0)).All();
      }
    }

    //
    // Validate
    //    

    public static void Validate(this INumberRangeDomain domain, Number value)
    {
      if (!domain.IsValid(value))
      {
        throw new ArgumentException($"Invalid value: {value}");
      }
    }

    /// <summary>
    /// Validate values
    /// </summary>    
    public static void Validate(this INumberRangeDomain domain, IEnumerable<Number> value)
    {
      foreach(Number v in value)
      {
        domain.Validate(v);
      }
    }
    
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

    //
    // Translate
    //

    public static NumberVector Translate(this INumberRangeDomain domain, NumberVector value, NumberTranslationVector delta)
    {
      return value.Zip(delta, (v, t) => domain.Translate(v ?? 0, t ?? 0)).ToVector();
    }

    public static NumberArray Translate(this INumberRangeDomain domain, NumberArray value, NumberTranslationVector delta)
    {
      if (delta.Length > value.Length)
      {
        throw new ArgumentException(nameof(delta));
      }

      return value.Zip(delta, (v, t) => domain.Translate(v.Value, t ?? 0)).ToNumberArray();
    }
  }
}
