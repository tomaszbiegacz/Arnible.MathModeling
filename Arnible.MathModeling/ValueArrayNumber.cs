namespace Arnible.MathModeling
{
  public static class ValueArrayNumber
  {
    /// <summary>
    /// Create empty array with given size
    /// </summary>
    /// <remarks>
    /// This could be improved to avoid array creation
    /// </remarks>
    public static ValueArray<Number> Zero(uint count)
    {
      return new Number[count];
    }
  }
}