using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.MathModeling.Geometry
{
  public class HypersphericalAnglePopulation : IEnumerable<HypersphericalAngleQuantified>
  {
    class DirectionComparerWithFitness : IComparer<HypersphericalAngleQuantified>
    {
      private readonly Dictionary<HypersphericalAngleQuantified, int> _fitness;
      private readonly IComparer<HypersphericalAngleQuantified> _deterministicOrder;

      public DirectionComparerWithFitness(IComparer<HypersphericalAngleQuantified> comparer, IEnumerable<HypersphericalAngleQuantified> angles)
      {
        _deterministicOrder = comparer ?? throw new ArgumentException(nameof(comparer));
        _fitness = angles.ToDictionary(d => d, d => 0);
      }

      public int Compare(HypersphericalAngleQuantified x, HypersphericalAngleQuantified y)
      {
        int result = -1 * _fitness[x].CompareTo(_fitness[y]);
        if (result != 0)
          return result;
        
        result = _deterministicOrder.Compare(x, y);
        if (result != 0)
          return result;

        return x.Angles.SequenceCompare(y.Angles);
      }

      public void IncreaseFitness(HypersphericalAngleQuantified d)
      {
        _fitness[d]++;
      }

      public void DecreaseFitness(HypersphericalAngleQuantified d)
      {
        _fitness[d]--;
      }
    }

    private readonly List<HypersphericalAngleQuantified> _angles;
    private readonly DirectionComparerWithFitness _comparer;

    public HypersphericalAnglePopulation(IEnumerable<HypersphericalAngleQuantified> angles, IComparer<HypersphericalAngleQuantified> comparer)
    {
      _angles = angles.ToList();
      _comparer = new DirectionComparerWithFitness(comparer, _angles);
      _angles.Sort(_comparer);
    }

    public IEnumerator<HypersphericalAngleQuantified> GetEnumerator() => _angles.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _angles.GetEnumerator();

    private void AddAngle(HypersphericalAngleQuantified direction)
    {
      int pos = _angles.BinarySearch(direction, _comparer);
      if (pos >= 0)
      {
        throw new InvalidOperationException("Found angle duplicate");
      }
      _angles.Insert(~pos, direction);
    }

    public void IncreaseFitness(HypersphericalAngleQuantified direction)
    {
      if (!_angles.Remove(direction))
      {
        throw new ArgumentException(nameof(direction));
      }
      _comparer.IncreaseFitness(direction);
      AddAngle(direction);
    }

    public void DecreaseFitness(HypersphericalAngleQuantified direction)
    {
      if (!_angles.Remove(direction))
      {
        throw new ArgumentException(nameof(direction));
      }
      _comparer.DecreaseFitness(direction);
      AddAngle(direction);
    }
  }
}
