using Arnible.MathModeling.Algebra;
using System;
using System.Collections.Generic;
using System.Linq;
using static Arnible.MathModeling.MetaMath;

namespace Arnible.MathModeling.Geometry
{
  public struct HypersphericalCoordinate : ICoordinate<HypersphericalCoordinate>
  {
    /// <summary>
    /// Angles:
    /// - from y axis to xy vector
    /// - from z axis to xyz vector
    /// </summary>
    /// <remarks>
    /// New dimension is being created by adding angle with π/4 (90 degres)
    /// </remarks>
    public HypersphericalAngleVector Angles { get; }

    public Number R { get; }

    public HypersphericalCoordinate(Number r, NumberVector angles)
    {
      R = r;
      Angles = new HypersphericalAngleVector(angles);

      if (r < 0)
      {
        throw new ArgumentException($"Negative r: {r}");
      }
      if (r == 0 && !angles.IsZero)
      {
        throw new ArgumentException($"For zero r, angles also has to be empty, got {angles}");
      }
    }

    public static implicit operator HypersphericalCoordinate(PolarCoordinate pc) => new HypersphericalCoordinate(pc.R, new NumberVector(pc.Φ));

    public override string ToString()
    {
      return new NumberVector(R).Concat(Angles).ToVector().ToString();
    }

    public uint DimensionsCount => (uint)Angles.Count + 1;

    public HypersphericalCoordinate AddDimension()
    {
      return new HypersphericalCoordinate(R, Angles.AddDimension());
    }

    public CartesianCoordinate ToCartesian()
    {
      var cartesianDimensions = new List<Number>();
      var replacement = R;
      foreach (var angle in Angles.Reverse())
      {
        cartesianDimensions.Add(replacement * Cos(angle));
        replacement *= Sin(angle);
      }
      cartesianDimensions.Add(replacement);
      cartesianDimensions.Reverse();

      return new CartesianCoordinate(cartesianDimensions.ToVector());
    }

    public IEnumerable<IDerivative1> DerivativeByRForCartesianCoordinates()
    {
      var result = new List<IDerivative1>();
      Number replacement = 1;
      foreach (var angle in Angles.Reverse())
      {
        result.Add(new Derivative1Value(replacement * Cos(angle)));
        replacement *= Sin(angle);
      }
      result.Add(new Derivative1Value(replacement));
      result.Reverse();

      return result;
    }

    public IEnumerable<HypersphericalAngleVector> CartesianCoordinatesAngles()
    {
      var anglesCount = Angles.Count;
      Number[] x = Enumerable.Repeat(HypersphericalAngleVector.AxisEraseAngle, anglesCount).ToArray();

      yield return new HypersphericalAngleVector(x.ToVector());
      for (uint i = 0; i < anglesCount; ++i)
      {
        x[i] = 0;
        yield return new HypersphericalAngleVector(x.ToVector());
        x[i] = HypersphericalAngleVector.AxisEraseAngle;
      }
    }
  }
}
