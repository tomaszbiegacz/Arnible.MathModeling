using Arnible.Linq;
using Arnible.MathModeling.Algebra;

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
      in ReadOnlyArray<Number> value,
      in ReadOnlyArray<Number> delta)
    {
      return value.AsList().ZipValue(
        col2: delta.AsList(), 
        merge: (v, t) => domain.GetValidTranslationRatio(v ?? 0, t ?? 0)).MinDefensive();
    }
    
    public static Number? GetMaximumValidTranslationRatio(
      this INumberRangeDomain domain,
      ReadOnlyArray<Number> value,
      in ReadOnlyArray<Number> transaction)
    {
      return value.AsList().ZipValue(
        col2: transaction.AsList(), 
        merge: (v, t) => domain.GetMaximumValidTranslationRatio(v ?? 0, t ?? 0)
      ).MinOrNone();
    }

    public static ReadOnlyArray<Number> GetValidTranslation(
      this INumberRangeDomain domain,
      in ReadOnlyArray<Number> value,
      in ReadOnlyArray<Number> delta)
    {
      Number ratio = GetValidTranslationRatio(domain, in value, in delta);
      return delta.AsList().Multiply(ratio);
    }
  }
}