using Arnible.MathModeling.Geometry;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Arnible.MathModeling.Test.Geometry
{
  public class HypersphericalAnglePopulationTests
  {
    class ComparerForTests1d : IComparer<HypersphericalAngleQuantified>
    {
      public int Compare(HypersphericalAngleQuantified x, HypersphericalAngleQuantified y) => -1 * x.Angles.Single().CompareTo(y.Angles.Single());
    }

    private static void AssertAngles(HypersphericalAnglePopulation population, params sbyte[] angles)
    {
      Assert.Equal(angles.Length, population.Count());
      for (int i = 0; i < angles.Length; ++i)
      {
        Assert.Equal(angles[i], population.ElementAt(i).Angles.Single());
      }
    }

    [Fact]
    public void HappyDay()
    {
      var population = new HypersphericalAnglePopulation(HypersphericalAngleQuantified.GetNonLinearDirections(anglesCount: 1, resolution: 2), new ComparerForTests1d());
      AssertAngles(population, 2, 1, 0, -1);

      population.IncreaseFitness(population.Last());
      AssertAngles(population, -1, 2, 1, 0);

      population.DecreaseFitness(population.ElementAt(1));
      AssertAngles(population, -1, 1, 0, 2);
    }
  }
}
