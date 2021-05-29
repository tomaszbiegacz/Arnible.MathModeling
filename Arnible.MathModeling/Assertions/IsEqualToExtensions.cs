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
      actual.Span.AssertSequenceEqualsTo(expected.Span);
    }
  }
}