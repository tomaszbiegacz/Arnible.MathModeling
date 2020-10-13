using System;
using System.Collections.Generic;
using Arnible.MathModeling.Algebra;

namespace Arnible.MathModeling.xunit
{
  public static class AssertNumeric
  {
    private static Number GetNearbyPoint(Number src, Sign direction, Number distance)
    {
      if (direction == Sign.None)
      {
        return src;
      }
      else
      {
        Number val = (int) direction * distance;
        return src + val;
      }
    }
    
    public static void AssertMinimum(
      Func<ValueArray<Number>, Number> func,
      ValueArray<Number> point,
      INumberRangeDomain valueDomain,
      Number distance)
    {
      AssertNumber.IsPositive(in distance);
      AssertNumber.IsTrue(valueDomain.IsValid(in point));
      
      Number pointValue = func(point);
      IUnmanagedArrayEnumerable<Sign> directions = new SignArrayEnumerable(point.Length);
      
      // skip zero point
      directions.MoveNext();
      
      while (directions.MoveNext())
      {
        IEnumerable<Sign> direction = directions;
        ValueArray<Number> newPoint = direction.Select((i, s) => GetNearbyPoint(point[i], s, distance)).ToValueArray();
        if (valueDomain.IsValid(in newPoint))
        {
          Number newValue = func(newPoint);
          if (newValue < pointValue)
          {
            throw new InvalidOperationException($"For distance {direction} in point {newPoint.ToString()} got value {newValue.ToString()} lower than {pointValue.ToString()}");
          }  
        }
      }
    }

    public static void AssertMinimum(
      Func<ValueArray<Number>, Number> func,
      ValueArray<Number> point,
      INumberRangeDomain valueDomain,
      Number distance,
      uint iterationsCount)
    {
      Number currentDistance = distance;
      for (uint i = 0; i < iterationsCount; ++i)
      {
        AssertMinimum(func: func, point: point, distance: currentDistance, valueDomain: valueDomain);
        currentDistance = currentDistance / 2;
      }
    }
  }
}