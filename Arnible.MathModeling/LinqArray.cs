using System;
using System.Collections.Generic;

namespace Arnible.MathModeling
{
  public static class LinqArray
  {
    public static IEnumerable<uint> Indexes<T>(this T[] arg)
    {
      return LinqEnumerable.RangeUint((uint)arg.Length);
    }

    public static IEnumerable<uint> IndexesWhere<T>(this T[] arg, Func<T, bool> predicate)
    {
      for(uint i=0; i<arg.Length; ++i)
      {
        if(predicate(arg[i]))
        {
          yield return i;
        }
      }
    }

    public static T[][] ToArrayJagged<T>(this T[,] twoDimensionalArray)
    {
      if (twoDimensionalArray == null)
      {
        throw new ArgumentNullException(nameof(twoDimensionalArray));
      }
      int rowsFirstIndex = twoDimensionalArray.GetLowerBound(0);
      int rowsLastIndex = twoDimensionalArray.GetUpperBound(0);
      int numberOfRows = rowsLastIndex + 1;

      int columnsFirstIndex = twoDimensionalArray.GetLowerBound(1);
      int columnsLastIndex = twoDimensionalArray.GetUpperBound(1);
      int numberOfColumns = columnsLastIndex + 1;

      T[][] jaggedArray = new T[numberOfRows][];
      for (int i = rowsFirstIndex; i <= rowsLastIndex; i++)
      {
        jaggedArray[i] = new T[numberOfColumns];

        for (int j = columnsFirstIndex; j <= columnsLastIndex; j++)
        {
          jaggedArray[i][j] = twoDimensionalArray[i, j];
        }
      }
      return jaggedArray;
    }

    public static T[][] ToArrayJaggedInversed<T>(this T[,] twoDimensionalArray)
    {
      if (twoDimensionalArray == null)
      {
        throw new ArgumentNullException(nameof(twoDimensionalArray));
      }
      int rowsFirstIndex = twoDimensionalArray.GetLowerBound(1);
      int rowsLastIndex = twoDimensionalArray.GetUpperBound(1);
      int numberOfRows = rowsLastIndex + 1;

      int columnsFirstIndex = twoDimensionalArray.GetLowerBound(0);
      int columnsLastIndex = twoDimensionalArray.GetUpperBound(0);
      int numberOfColumns = columnsLastIndex + 1;

      T[][] jaggedArray = new T[numberOfRows][];
      for (int i = rowsFirstIndex; i <= rowsLastIndex; i++)
      {
        jaggedArray[i] = new T[numberOfColumns];

        for (int j = columnsFirstIndex; j <= columnsLastIndex; j++)
        {
          jaggedArray[i][j] = twoDimensionalArray[j, i];
        }
      }
      return jaggedArray;
    }
  }
}
