namespace Arnible.Assertions
{
  public static class IsNullExtensions
  {
    public static void AssertIsNull<T>(this T? actual) where T: struct
    {
      if(actual.HasValue)
      {
        throw new AssertException($"Expected null got {actual}");
      }
    }
    
    public static void AssertIsNull<T>(this T? actual) where T: class
    {
      if(actual is not null)
      {
        throw new AssertException($"Expected null got {actual}");
      }
    }
  }
}