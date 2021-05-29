using Arnible.MathModeling.Algebra;
using System;
using Arnible.Assertions;
using Arnible.Linq;

namespace Arnible.MathModeling.Geometry
{
  public static class INumberRangeDomainExtensions
  {
    public static bool IsValidTranslation(
      this INumberRangeDomain domain,
      in HypersphericalCoordinate value,
      in ReadOnlySpan<Number> delta)
    {
      Span<Number> translation = stackalloc Number[delta.Length];
      delta.CopyTo(translation);

      var coordinates = value.Translate(in translation).ToCartesianView().Coordinates;
      return coordinates.AsList().AllWithDefault(v => domain.IsValid(v));
    }
    
    public static void GetValidTranslationForLastAngle(
      this INumberRangeDomain domain,
      in HypersphericalCoordinate value,
      in Span<Number> delta)
    {
      value.Angles.Length.AssertIsEqualTo(delta.Length);
      delta.Count((in Number v) => v != 0).AssertIsEqualTo(1);
      delta[^1].AssertIsNotEqualTo(0, "Something went wrong, only last angle should be not empty");

      ushort anglePos = (ushort)(delta.Length - 1);
      Number ratio = domain.GetValidTranslationRatioForLastAngle(
        radius: value.R,
        currentAngle: value.Angles[anglePos],
        angleDelta: in delta[anglePos]);

      delta.MultiplyInPlace(ratio);
    }
    
    public static HypersphericalCoordinate Translate(
      this INumberRangeDomain domain,
      in HypersphericalCoordinate value,
      in ReadOnlySpan<Number> delta)
    {
      Span<Number> validTranslation = stackalloc Number[delta.Length];
      delta.CopyTo(validTranslation);
      domain.GetValidTranslationForLastAngle(value, in validTranslation);
      
      return value.Translate(in validTranslation);
    }
  }
}
