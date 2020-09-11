using Arnible.MathModeling.Algebra;
using System;
using System.Collections.Generic;

namespace Arnible.MathModeling.Geometry
{
  public static class INumberRangeDomainExtensions
  {
    //
    // IsValidTranslation
    //

    public static bool IsValidTranslation(
      this INumberRangeDomain domain,
      in HypersphericalCoordinate value,
      in HypersphericalAngleTranslationVector delta)
    {
      domain.Validate(value.ToCartesianView().Coordinates);
      NumberVector coordinates = delta.Translate(in value).ToCartesianView().Coordinates;
      return coordinates.All(v => domain.IsValid(v));
    }

    //
    // GetValidTranslation
    //    

    public static HypersphericalAngleTranslationVector GetValidTranslation(
      this INumberRangeDomain domain,
      in HypersphericalCoordinate value,
      HypersphericalAngleTranslationVector delta)
    {
      domain.Validate(value.ToCartesianView().Coordinates);
      IReadOnlyList<uint> nonZeroAngles = LinqEnumerable.RangeUint(0, delta.Length)
        .Where(i => delta[i] != 0)
        .ToReadOnlyList();
      
      if (nonZeroAngles.Count != 1)
      {
        throw new ArgumentException("Exactly one angle has to be non-negative. Other options are not yet supported.");
      }
      if (nonZeroAngles[0] != delta.Length - 1)
      {
        throw new InvalidOperationException($"Something went wrong, only last angle should be not empty, in {delta}");
      }

      uint anglePos = delta.Length - 1;
      Number ratio = domain.GetValidTranslationRatioForLastAngle(
        radius: value.R,
        currentAngle: value.Angles.GetOrDefault(anglePos),
        angleDelta: in delta[anglePos]);

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

    //
    // Translate
    //

    public static HypersphericalCoordinate Translate(
      this INumberRangeDomain domain,
      in HypersphericalCoordinate value,
      in HypersphericalAngleTranslationVector delta)
    {
      HypersphericalAngleTranslationVector tr = domain.GetValidTranslation(in value, delta);
      return new HypersphericalCoordinate(value.R, tr.Translate(value.Angles));
    }
  }
}
