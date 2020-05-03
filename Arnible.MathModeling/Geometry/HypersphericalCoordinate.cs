using Arnible.MathModeling.Algebra;
using System;
using System.Collections.Generic;
using System.Linq;
using static Arnible.MathModeling.MetaMath;

namespace Arnible.MathModeling.Geometry
{
  public readonly struct HypersphericalCoordinate : ICoordinate<HypersphericalCoordinate>
  {
    /// <summary>
    /// Angles:
    /// - from x axis to r(xy) range over [-π, π]
    /// - from xy plane to r(xyz) range over [-π/2, π/2]
    /// - etc..
    /// </summary>
    /// <remarks>
    /// New (hidden) dimension is being created by adding 0 angle
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
        cartesianDimensions.Add(replacement * Sin(angle));
        replacement *= Cos(angle);
      }
      cartesianDimensions.Add(replacement);
      cartesianDimensions.Reverse();

      return new CartesianCoordinate(cartesianDimensions.ToVector());
    }

    public IEnumerable<IDerivative1> DerivativeByRForCartesianCoordinates()
    {
      return DerivativeByRForCartesianCoordinates(Angles);
    }

    public static IEnumerable<IDerivative1> DerivativeByRForCartesianCoordinates(HypersphericalAngleVector angles)
    {
      var result = new List<IDerivative1>();
      Number replacement = 1;
      foreach (var angle in angles.Reverse())
      {
        result.Add(new Derivative1Value(replacement * Sin(angle)));
        replacement *= Cos(angle);
      }
      result.Add(new Derivative1Value(replacement));
      result.Reverse();

      return result;
    }

    public IEnumerable<HypersphericalAngleVector> CartesianCoordinatesAngles()
    {
      var anglesCount = Angles.Count;
      Number[] x = Enumerable.Repeat<Number>(0, anglesCount).ToArray();

      yield return new HypersphericalAngleVector(x.ToVector());
      for (uint i = 0; i < anglesCount; ++i)
      {
        x[i] = Angle.RightAngle;
        yield return new HypersphericalAngleVector(x.ToVector());
        x[i] = 0;
      }
    }
  }
}
