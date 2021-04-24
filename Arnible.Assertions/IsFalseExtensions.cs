namespace Arnible.Assertions
{
  public static class IsFalseExtensions
  {
    public static void AssertIsFalse(this bool value)
    {
      if(value)
      {
        throw new AssertException("Condition is not met");
      }
    }
  }
}