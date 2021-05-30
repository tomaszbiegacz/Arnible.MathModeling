using Arnible.MathModeling;
using Arnible.MathModeling.Geometry;

namespace Arnible.Assertions
{
  public static class IsEqualToExtensions
  {
    public static void AssertIsEqualTo(in this Number actual, double expected)
    {
      actual.AssertIsEqualTo((Number)expected);
    }
    
    public static void AssertIsEqualTo(in this HypersphericalAngleVector actual, in HypersphericalAngleVector expected)
    {
      actual.Equals(in expected).AssertIsTrue();
    }
    
    public static void AssertIsEqualTo(in this HypersphericalCoordinate actual, in HypersphericalCoordinate expected)
    {
      actual.Angles.AssertIsEqualTo(expected.Angles);
      actual.R.AssertIsEqualTo(expected.R);
    }
  }
}