﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Arnible.Assertions
{
  public static class SequenceEqualsToExtensions
  {
    public static void AssertSequenceEqualsTo<T>(this IEnumerable<T> actual, in ReadOnlySpan<T> expected)
      where T: IEquatable<T>
    {
      var actualMaterialized = actual.ToArray();
      actualMaterialized.Length.AssertIsEqualTo(expected.Length);
      
      for(ushort i=0; i<expected.Length; ++i)
      {
        if(!actualMaterialized[i].Equals(expected[i]))
        {
          throw new AssertException(
            $"At position {i} expected {expected[i]} got {actualMaterialized[i]}", 
            AssertException.ToString(actualMaterialized)
            );
        }
      }
    }

    public static void AssertSequenceEqualsTo<T>(in this ReadOnlySpan<T> actual, in ReadOnlySpan<T> expected)
      where T: IEquatable<T>
    {
      actual.Length.AssertIsEqualTo(expected.Length);
      for(ushort i=0; i<actual.Length; ++i)
      {
        if(!actual[i].Equals(expected[i]))
        {
          throw new AssertException($"At position {i} expected {expected[i]} got {actual[i]}");
        }
      }
    }
    
    public static void AssertSequenceEqualsTo<T>(in this Span<T> actual, in ReadOnlySpan<T> expected)
      where T: IEquatable<T>
    {
      AssertSequenceEqualsTo((ReadOnlySpan<T>)actual, in expected);
    }
  }
}