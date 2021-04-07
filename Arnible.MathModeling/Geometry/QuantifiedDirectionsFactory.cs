using System;
using System.Collections.Generic;
using Arnible.Linq;

namespace Arnible.MathModeling.Geometry
{
  class QuantifiedDirectionsFactory
  {
    private readonly HypersphericalAngleQuantified.Factory _factory;
    private readonly uint _angleCount;

    public QuantifiedDirectionsFactory(HypersphericalAngleQuantified.Factory factory, uint anglesCount)
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
          var prefix = _factory.Axis(axisAngle).ToArray();
          foreach (var sequence in _factory.AnglesWithoutRightAngle().ToSequencesWithReturning(_angleCount - axisAngle))
          {
            yield return _factory.Create(prefix.Concat(sequence));
          }
        }
        yield return _factory.Create(_factory.Axis(_angleCount));
      }
    }

    private IEnumerable<HypersphericalAngleQuantified> WithoutRightAnglePrefix => _factory.AnglesWithoutRightAngle().ToSequencesWithReturning(_angleCount).Select(a => _factory.Create(a));

    public IEnumerable<HypersphericalAngleQuantified> Angles => WithRightAnglePrefix.Concat(WithoutRightAnglePrefix);
  }
}
