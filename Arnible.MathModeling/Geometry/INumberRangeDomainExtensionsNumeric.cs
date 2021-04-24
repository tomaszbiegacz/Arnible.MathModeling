using Arnible.Linq;

namespace Arnible.MathModeling.Geometry
{
  public static class INumberRangeDomainExtensionsNumeric
  {
    public static Number GetValidTranslationRatio(
      this INumberRangeDomain domain,
      ReadOnlyArray<Number> value,
      ReadOnlyArray<Number> delta)
    {
      return value.AsList().ZipValue(
        col2: delta.AsList(), 
        merge: (v, t) => domain.GetValidTranslationRatio(v ?? 0, t ?? 0)).MinDefensive();
    }
    
    private static Number GetValidTranslationRatio(
      INumberRangeDomain domain,
      in NumberVector value,
      in NumberVector delta)
    {
      return value.GetInternalEnumerable().ZipValue(
        col2: delta.GetInternalEnumerable(), 
        merge: (v, t) => domain.GetValidTranslationRatio(v ?? 0, t ?? 0)).MinDefensive();
    }
    
    public static Number? GetMaximumValidTranslationRatio(
      this INumberRangeDomain domain,
      ReadOnlyArray<Number> value,
      in NumberVector transaction)
    {
      return value.AsList().ZipValue(
        col2: transaction.GetInternalEnumerable(), 
        merge: (v, t) => domain.GetMaximumValidTranslationRatio(v ?? 0, t ?? 0)
      ).MinOrNone();
    }

    public static NumberVector GetValidTranslation(
      this INumberRangeDomain domain,
      in NumberVector value,
      in NumberVector delta)
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
  }
}