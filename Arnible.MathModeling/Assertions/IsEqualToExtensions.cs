using Arnible.MathModeling;

namespace Arnible.Assertions
{
  public static class IsEqualToExtensions
  {
    public static void AssertIsEqualTo(in this Number actual, double expected)
    {
      actual.AssertIsEqualTo((Number)expected);
    }
  }
}