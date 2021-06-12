using System;
using System.Collections.Generic;

namespace Arnible.Linq
{
  public static class ToArrayExtensions
  {
    public static T[] ToArray<T>(this IEnumerable<T> source)
    {
      return System.Linq.Enumerable.ToArray(source);
    }

    public static T[][] ToArrayJagged<T>(this T[,] twoDimensionalArray)
    {
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

    public static T[][] ToArrayJaggedInverted<T>(this T[,] twoDimensionalArray)
    {
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