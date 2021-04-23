using System;
using System.Collections.Generic;
using Arnible.Linq;

namespace Arnible.MathModeling
{
  public static class INumberRangeDomainExtensions
  {
    public static bool IsValid(this INumberRangeDomain domain, ReadOnlyArray<Number> values)
    {
      return values.AsList().AllWithDefault(v => domain.IsValid(in v));
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

    //
    // Validate
    //    

    public static void Validate(
      this INumberRangeDomain domain, 
      in Number value)
    {
      if (!domain.IsValid(in value))
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
  }
}
