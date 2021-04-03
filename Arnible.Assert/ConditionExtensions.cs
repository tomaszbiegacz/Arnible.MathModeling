namespace Arnible.Assert
{
  public static class ConditionExtensions
  {
    public static void AssertIsTrue(this bool value)
    {
      if(!value)
      {
        throw new AssertException("Condition is not met");
      }
    }
    
    public static void AssertIsFalse(this bool value)
    {
      if(value)
      {
        throw new AssertException("Condition is not met");
      }
    }
  }
}