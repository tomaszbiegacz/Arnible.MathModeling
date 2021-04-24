namespace Arnible.Assertions
{
  public static class IsTrueExtensions
  {
    public static void AssertIsTrue(this bool value)
    {
      if(!value)
      {
        throw new AssertException("Condition is not met");
      }
    }
  }
}