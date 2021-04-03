namespace Arnible.Assert
{
  public static class RangeExtensions
  {
    public static void AssertIsGreaterThan(this ushort actual, int baseValue)
    {
      if(baseValue >= actual)
      {
        throw new AssertException($"Expected greater than {baseValue}, got {actual}");
      }
    }
  }
}