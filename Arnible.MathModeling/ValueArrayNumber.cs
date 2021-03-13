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
    
    public static (ValueArray<Number>, ValueArray<Number>) SplitValuesBySign(this ValueArray<Number> src)
    {
      Number[] positive = new Number[src.Length];
      Number[] negative = new Number[src.Length];
      
      for(uint i=0; i<src.Length; ++i)
      {
        ref readonly Number value = ref src[i];
        if(value >= 0)
        {
          positive[i] = value;
        }
        else
        {
          negative[i] = value;
        }
      }
      
      return (negative, positive);
    }
    
    public static ValueArray<Number> GetNegativeValues(this ValueArray<Number> src)
    {
      Number[] negative = new Number[src.Length];
      
      for(uint i=0; i<src.Length; ++i)
      {
        ref readonly Number value = ref src[i];
        if(value < 0)
        {
          negative[i] = value;
        }
      }
      
      return negative;
    }
    
    public static ValueArray<Number> GetPositiveValues(this ValueArray<Number> src)
    {
      Number[] positive = new Number[src.Length];
      
      for(uint i=0; i<src.Length; ++i)
      {
        ref readonly Number value = ref src[i];
        if(value > 0)
        {
          positive[i] = value;
        }
      }
      
      return positive;
    }
  }
}