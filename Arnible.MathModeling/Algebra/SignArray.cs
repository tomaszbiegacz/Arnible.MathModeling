using System;
using System.Collections.Generic;
using Arnible.Linq;

namespace Arnible.MathModeling.Algebra
{
  readonly struct SignArray : IComparable<SignArray>
  {
    public SignArray(IEnumerable<Sign> sings)
    {
      Values = sings.ToUnmanagedArray();
    }

    /*
     * IComparable
     */

    public int CompareTo(SignArray other)
    {
      int byLength = Values.Length.CompareTo(other.Values.Length);
      if (byLength != 0)
      {
        return byLength;
      }
      
      uint valueCount = Values.Where(s => s != 0).Count();
      uint valueCountOther = other.Values.Where(s => s != 0).Count();
      int byValueCount = valueCount.CompareTo(valueCountOther);
      if (byValueCount != 0)
      {
        return byValueCount;
      }

      for (uint iPos = 0; iPos < Values.Length; iPos++)
      {
        uint i = Values.Length - 1 - iPos;
        Sign v1 = Values[i];
        Sign v2 = other.Values[i];
        int byValue = ((int)v1).CompareTo((int)v2);
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

    public UnmanagedArray<Sign> Values { get; }
    
    /*
     * Operations
     */
    
    public bool GetIsOrthogonal()
    {
      uint negativeCount = Values.Where(s => s < 0).Count();
      if (negativeCount == 0)
      {
        // only positive direction
        return true;
      }

      Sign lastNonZero = Values.Where(s => s != 0).Last();
      return lastNonZero > 0;
    }
  }
}
