using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class NumericFormatting
  {
    private static readonly char[] _superscriptDigits = new[] { '⁰', '¹', '²', '³', '⁴', '⁵', '⁶', '⁷', '⁸', '⁹' };

    private static IEnumerable<char> ToSuperscriptReverseString(uint number)
    {
      if (number == 0)
      {
        yield return _superscriptDigits[0];
      }        
      else
      {
        while(number >= _superscriptDigits.Length)
        {
          yield return _superscriptDigits[number % _superscriptDigits.Length];
          number = (uint)(number / _superscriptDigits.Length);
        }
        yield return _superscriptDigits[number];
      }
    }

    public static string ToSuperscriptString(this uint number) => string.Concat(ToSuperscriptReverseString(number).Reverse());
  }
}
