namespace Arnible.MathModeling
{
  public static class ValueArrayExtensionsNotFormal
  {
    //
    // Number specialization
    //

    public static Number MedianDefensive(in this ValueArray<Number> src)
    {
      return src.GetInternalEnumerable().MedianDefensive();
    }
    
    public static Number MinDefensive(in this ValueArray<Number> src)
    {
      return src.GetInternalEnumerable().MinDefensive();
    }

    public static Number MaxDefensive(in this ValueArray<Number> src)
    {
      return src.GetInternalEnumerable().MaxDefensive();
    }
  }
}