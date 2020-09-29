using System;
using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  public static class INumberRangeDomainExtensions
  {
    public static bool IsValid(this INumberRangeDomain domain, in ValueArray<Number> values)
    {
      return values.All(v => domain.IsValid(v));
    }
    
    //
    // IsValidTranslation
    //

    public static bool IsValidTranslation(
      this INumberRangeDomain domain, 
      in Number value, 
      in Number delta)
    {
      domain.Validate(value);
      return domain.IsValid(value + delta);
    }

    public static bool IsValidTranslation(
      this INumberRangeDomain domain, 
      in NumberVector value, 
      in NumberTranslationVector delta)
    {
      return value.GetInternalEnumerable().Zip(
        col2: delta.GetInternalEnumerable(), 
        merge: (v, t) => domain.IsValidTranslation(v ?? 0, t ?? 0)).All();
    }

    public static bool IsValidTranslation(
      this INumberRangeDomain domain,
      in ValueArray<Number> value,
      in NumberTranslationVector delta)
    {
      if (delta.Length > value.Length)
      {
        return false;
      }
      else
      {
        return value.GetInternalEnumerable().Zip(
          col2: delta.GetInternalEnumerable(), 
          merge: (v, t) => domain.IsValidTranslation(v ?? throw new ArgumentException(nameof(value)), t ?? 0)).All();
      }
    }

    //
    // Validate
    //    

    public static void Validate(
      this INumberRangeDomain domain, 
      in Number value)
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
      foreach (Number v in value)
      {
        domain.Validate(v);
      }
    }

    public static void Validate(this INumberRangeDomain domain, NumberVector value)
    {
      Validate(domain, value.GetInternalEnumerable());
    }

    //
    // Translate
    //

    public static NumberVector Translate(
      this INumberRangeDomain domain, 
      in NumberVector value, 
      in NumberTranslationVector delta)
    {
      return value.GetInternalEnumerable().Zip(
        col2: delta.GetInternalEnumerable(), 
        merge: (v, t) => domain.Translate(v ?? 0, t ?? 0)).ToVector();
    }

    public static ValueArray<Number> Translate(
      this INumberRangeDomain domain, 
      in ValueArray<Number> value, 
      in NumberTranslationVector delta)
    {
      if (delta.Length > value.Length)
      {
        throw new ArgumentException(nameof(delta));
      }

      return value.GetInternalEnumerable().Zip(
        col2: delta.GetInternalEnumerable(), 
        merge: (v, t) => domain.Translate(v ?? throw new ArgumentException(nameof(value)), t ?? 0)).ToValueArray();
    }
  }
}
