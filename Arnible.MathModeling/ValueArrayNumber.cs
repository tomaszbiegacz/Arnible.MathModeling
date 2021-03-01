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
    
    public static bool IsZero(in this ValueArray<Number> src)
    {
      for(uint i=0; i<src.Length; i++)
      {
        if(src[i] != 0)
        {
          return false;
        }
      }
      return true;
    }
  }
}