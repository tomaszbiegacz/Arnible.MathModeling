namespace Arnible.Assertions
{
  public static class IsTrueExtensions
  {
    public static void AssertIsTrue(this bool value, string? message = null)
    {
      if(!value)
      {
        throw new AssertException(message ?? "Condition is not met");
      }
    }
  }
}