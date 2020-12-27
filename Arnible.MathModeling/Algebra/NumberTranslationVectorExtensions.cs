using System;

namespace Arnible.MathModeling.Algebra
{
  public static class NumberTranslationVectorExtensions
  {
    public static NumberTranslationVector GetNormalized(this in NumberTranslationVector src)
    {
      Number lengthSquare = src.GetLengthSquare();
      if (lengthSquare == 0 || lengthSquare == 1)
      {
        return src;
      }
      else
      {
        double vectorLength = Math.Sqrt((double)lengthSquare);
        return src.GetInternalEnumerable().Select(x => x / vectorLength).ToVector();  
      }
    }
  }
}