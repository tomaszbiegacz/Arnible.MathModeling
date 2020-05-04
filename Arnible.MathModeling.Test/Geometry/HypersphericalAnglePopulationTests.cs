using System.Collections.Generic;
using Xunit;

namespace Arnible.MathModeling.Geometry.Test
{
  public class HypersphericalAnglePopulationTests
  {
    class ComparerForTests1d : IComparer<HypersphericalAngleQuantified>
    {
      public int Compare(HypersphericalAngleQuantified x, HypersphericalAngleQuantified y) => -1 * x.Angles.Single().CompareTo(y.Angles.Single());
    }

    private static void AssertAngles(HypersphericalAnglePopulation population, params sbyte[] angles)
    {
      Assert.Equal((uint)angles.Length, population.Count());
      for (uint i = 0; i < angles.Length; ++i)
      {
        Assert.Equal(angles[i], population[i].Angles.Single());
      }
    }

    [Fact]
    public void HappyDay_1_2()
    {
      var population = new HypersphericalAnglePopulation(HypersphericalAngleQuantified.GetNonLinearDirections(anglesCount: 1, resolution: 2), new ComparerForTests1d());
      AssertAngles(population, 2, 1, 0, -1);

      population.IncreaseFitness(population.Last());
      AssertAngles(population, -1, 2, 1, 0);

      population.DecreaseFitness(population[1]);
      AssertAngles(population, -1, 1, 0, 2);
    }
  }
}
