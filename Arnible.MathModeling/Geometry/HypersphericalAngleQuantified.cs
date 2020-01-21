using Arnible.MathModeling.Algebra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.MathModeling.Geometry
{
  public struct HypersphericalAngleQuantified : IEquatable<HypersphericalAngleQuantified>
  {
    class InnerFactory
    {
      private uint _id = 0;
      private readonly byte _resolution;

      public InnerFactory(byte resolution)
      {
        if (resolution == 0)
          throw new ArgumentException(nameof(resolution));

        _resolution = resolution;
      }

      public sbyte LowestAngle => (sbyte)(-1 * _resolution + 1);

      public sbyte RightAngle => (sbyte)_resolution;

      public IEnumerable<sbyte> AnglesWithoutRightAngle()
      {
        for (sbyte angle = LowestAngle; angle < RightAngle; ++angle)
        {
          yield return angle;
        }
      }
      
      public IEnumerable<sbyte> Axis(uint angleCount)
      {
        if (angleCount == 0)
        {
          throw new ArgumentException(nameof(angleCount));
        }
        for (uint i = 1; i < angleCount; ++i)
        {
          yield return 0;
        }
        yield return RightAngle;
      }

      public HypersphericalAngleQuantified Create(IEnumerable<sbyte> angles) => new HypersphericalAngleQuantified(angles, _resolution, ++_id);
    }

    class NonLinearDirectionsFactory
    {
      private readonly InnerFactory _factory;
      private readonly uint _angleCount;

      public NonLinearDirectionsFactory(InnerFactory factory, uint anglesCount)
      {
        if (anglesCount == 0)
          throw new ArgumentException(nameof(anglesCount));

        _angleCount = anglesCount;
        _factory = factory;
      }

      private IEnumerable<HypersphericalAngleQuantified> WithRightAnglePrefix
      {
        get
        {
          for (uint axisAngle = 1; axisAngle < _angleCount; ++axisAngle)
          {
            var prefix = _factory.Axis(axisAngle);
            foreach (var sequence in _factory.AnglesWithoutRightAngle().ToSequncesWithReturning(_angleCount - axisAngle))
            {
              yield return _factory.Create(prefix.Concat(sequence));
            }
          }
          yield return _factory.Create(_factory.Axis(_angleCount));
        }
      }

      private IEnumerable<HypersphericalAngleQuantified> WithoutRightAnglePrefix => _factory.AnglesWithoutRightAngle().ToSequncesWithReturning(_angleCount).Select(a => _factory.Create(a));

      public IEnumerable<HypersphericalAngleQuantified> Angles => WithRightAnglePrefix.Concat(WithoutRightAnglePrefix);
    }

    public static IEnumerable<HypersphericalAngleQuantified> GetNonLinearDirections(byte anglesCount, byte resolution)
    {
      var factory = new NonLinearDirectionsFactory(new InnerFactory(resolution), anglesCount);
      return factory.Angles;
    }

    private readonly byte _rightAngleResolution;
    private readonly sbyte[] _angles;

    private HypersphericalAngleQuantified(IEnumerable<sbyte> angles, byte rightAngleResolution, uint id)
    {
      Id = id;
      _rightAngleResolution = rightAngleResolution;
      _angles = angles.ToArray();
      UsedDirectionsCount = GetUsedDirectionsCount(_angles, rightAngleResolution);
      
    }

    private static byte GetUsedDirectionsCount(IReadOnlyList<sbyte> angles, byte rightAngleResolution)
    {
      byte result = 0;
      if (angles.Any())
      {
        int firstAnglePos = angles.IndexOf(a => a == rightAngleResolution);
        if (firstAnglePos >= 0)
        {
          result = 1;
          firstAnglePos++;
        }
        else
        {
          if (angles.First() == 0)
          {
            result = 1;
          }
          firstAnglePos = 0;
        }
        result += (byte)angles.Skip(firstAnglePos).Count(a => a != 0);
      }
      return result;
    }

    public uint Id { get; }

    public byte UsedDirectionsCount { get; }

    public IEnumerable<sbyte> Angles => _angles ?? Enumerable.Empty<sbyte>();

    public bool Equals(HypersphericalAngleQuantified other) => Id == other.Id;

    public override bool Equals(object obj)
    {
      if (obj is HypersphericalAngleQuantified objCast)
        return Equals(objCast);
      else
        return false;
    }

    public static bool operator ==(HypersphericalAngleQuantified a, HypersphericalAngleQuantified b) => a.Equals(b);
    public static bool operator !=(HypersphericalAngleQuantified a, HypersphericalAngleQuantified b) => !a.Equals(b);

    public override int GetHashCode() => Id.GetHashCode();

    public override string ToString()
    {
      return "[" + string.Join(",", Angles) + "]";
    }

    public HypersphericalAngleVector ToVector()
    {
      if (!Angles.Any())
        return default;

      const double RightAngle = Math.PI / 2;
      Number step = RightAngle / _rightAngleResolution;
      return new HypersphericalAngleVector(new NumberVector(_angles.Select(v => v * step)));
    }
  }
}
