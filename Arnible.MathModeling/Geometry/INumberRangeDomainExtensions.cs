using System;
using Arnible.Assertions;

namespace Arnible.MathModeling.Geometry
{
  public static class INumberRangeDomainExtensions
  {
    public static bool IsValidTranslation(
      this INumberRangeDomain domain,
      in HypersphericalCoordinate value,
      in HypersphericalAngleVector delta)
    {
      Span<Number> bufferValue = stackalloc Number[value.Angles.Length];
      HypersphericalCoordinate translatedValue = value.Clone(in bufferValue);
      translatedValue.TranslateSelf(in delta);
      
      Span<Number> coordinates = stackalloc Number[value.Angles.Length + 1];
      translatedValue.ToCartesian(in coordinates);
      return domain.IsValid(in coordinates);
    }
    
    public static void GetValidTranslationForLastAngle(
      this INumberRangeDomain domain,
      in HypersphericalCoordinate value,
      in HypersphericalAngleVector delta)
    {
      value.Angles.Length.AssertIsEqualTo(delta.Length);
      delta.IsOrthogonal().AssertIsTrue();
      delta.Span[^1].AssertIsNotEqualTo(0, "Something went wrong, only last angle should be not empty");

      ushort anglePos = (ushort)(delta.Length - 1);
      Number ratio = domain.GetValidTranslationRatioForLastAngle(
        radius: value.R,
        currentAngle: value.Angles.Span[anglePos],
        angleDelta: in delta.Span[anglePos]);

      delta.ScaleSelf(ratio);
    }
  }
}
