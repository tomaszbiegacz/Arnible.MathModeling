using System;
using System.Collections.Generic;

namespace Arnible.MathModeling.Algebra
{
  readonly struct SignArray : IComparable<SignArray>
  {
    private readonly uint _valueAbsCount;
    private readonly uint _valueNegativeCount;

    public SignArray(IEnumerable<sbyte> sings)
    {
      Signs = sings.ToValueArray();
      _valueAbsCount = sings.Where(s => s != 0).Count();
      _valueNegativeCount = sings.Where(s => s < 0).Count();
    }

    /*
     * IComparable
     */

    public int CompareTo(SignArray other)
    {
      int byAbsCount = _valueAbsCount.CompareTo(other._valueAbsCount);
      if (byAbsCount != 0)
      {
        return byAbsCount;
      }

      int byNegativeCount = _valueNegativeCount.CompareTo(other._valueNegativeCount);
      if (byNegativeCount != 0)
      {
        return byNegativeCount;
      }

      int byLength = Signs.Length.CompareTo(other.Signs.Length);
      if (byLength != 0)
      {
        return byLength;
      }

      for (uint i = Signs.Length - 1; i >= 0; --i)
      {
        int byValue = Signs[i].CompareTo(other.Signs[i]);
        if (byValue != 0)
        {
          return byValue;
        }
      }

      return 0;
    }

    /*
     * Properties
     */

    public bool IsOrthogonal
    {
      get
      {
        return _valueAbsCount == 0 || _valueAbsCount != _valueNegativeCount;
      }
    }

    public ValueArray<sbyte> Signs { get; }
  }
}
