using System;
using System.Collections.Generic;
using Arnible.Linq;

namespace Arnible.MathModeling.Geometry
{
  public interface ICartesianCoordinateBlackWhiteMap
  {
    uint DimensionsCount { get; }
    
    uint MarkedPointsCount { get; }

    bool IsMarked(in ValueArray<Number> point);
  }
  
  public class ConcurrentCartesianCoordinateBlackWhiteMap : ICartesianCoordinateBlackWhiteMap
  {
    private readonly ValueArray<Number> _leftBottomMapCorner;
    private readonly ValueArray<Number> _rightTopMapCorner;
    private readonly ValueArray<Number> _normalizedStep;
    private readonly byte _precision;
    private readonly List<byte[]> _points;

    class PointsComparer : IComparer<byte[]>
    {
      public static PointsComparer Current { get; } = new PointsComparer();
      
      public int Compare(byte[] x, byte[] y)
      {
        if (x.Length != y.Length)
        {
          throw new ArgumentException(nameof(y));
        }

        for (uint i = 0; i < x.Length; ++i)
        {
          if (x[i] != y[i])
          {
            // we know the result
            return x[i].CompareTo(y[i]);
          }
        }

        return 0;
      }
    }

    public ConcurrentCartesianCoordinateBlackWhiteMap(
      in ValueArray<Number> leftBottomMapCorner,
      in ValueArray<Number> rightTopMapCorner,
      byte precision)
    {
      if (leftBottomMapCorner.Length == 0)
      {
        throw new ArgumentException(nameof(leftBottomMapCorner));
      }
      if (leftBottomMapCorner.Length != rightTopMapCorner.Length || leftBottomMapCorner == rightTopMapCorner)
      {
        throw new ArgumentException(nameof(rightTopMapCorner));
      }
      if (precision == 0)
      {
        throw new ArgumentException(nameof(precision));
      }

      _leftBottomMapCorner = leftBottomMapCorner;
      _rightTopMapCorner = rightTopMapCorner;
      _precision = precision;
      _normalizedStep = rightTopMapCorner.Substract(leftBottomMapCorner).Select(v => v / precision).ToArray();
      _points = new List<byte[]>();
    }
    
    //
    // Properties
    //

    public uint DimensionsCount => _leftBottomMapCorner.Length;

    public uint MarkedPointsCount
    {
      get
      {
        lock (_points)
        {
          return (uint) _points.Count;
        }
      }
    }
    
    //
    // Query operations
    //

    private IEnumerable<byte> NormalizeCoordinate(ValueArray<Number> point)
    {
      if (point.Length != DimensionsCount)
      {
        throw new ArgumentException(nameof(point));
      }

      for (uint i = 0; i < point.Length; ++i)
      {
        Number v = point[i];
        Number left = _leftBottomMapCorner[i];
        Number right = _rightTopMapCorner[i];
        
        if (v < left || v > right)
        {
          throw new ArgumentException(nameof(point));
        }

        if (v == right)
        {
          yield return (byte)(_precision - 1);
        }
        else
        {
          if (v == left)
          {
            yield return 0;
          }
          else
          {
            Number normalized = (v - left) / _normalizedStep[i];
            yield return (byte) Math.Floor((double)normalized);  
          }
        }
      }
    }
    
    public bool IsMarked(in ValueArray<Number> point)
    {
      byte[] normalizedPoint = System.Linq.Enumerable.ToArray(NormalizeCoordinate(point));
      lock (_points)
      {
        int pos = _points.BinarySearch(normalizedPoint, PointsComparer.Current);
        return pos >= 0;
      }
    }
    
    //
    // Operations
    //

    private bool MarkPoint(byte[] normalizedPoint)
    {
      int pos = _points.BinarySearch(normalizedPoint, PointsComparer.Current);
      if (pos < 0)
      {
        _points.Insert(~pos, normalizedPoint);
        return true;
      }
      else
      {
        return false;
      }
    }

    public bool MarkPoint(in ValueArray<Number> point)
    {
      byte[] normalizedPoint = System.Linq.Enumerable.ToArray(NormalizeCoordinate(point));
      lock (_points)
      {
        return MarkPoint(normalizedPoint);
      }
    }
    
    public void MarkPoints(IEnumerable<ValueArray<Number>> points)
    {
      IReadOnlyList<byte[]> normalizedPoints = points
        .Select(p => System.Linq.Enumerable.ToArray(NormalizeCoordinate(p)))
        .ToArray();
      
      lock (_points)
      {
        foreach (byte[] p in normalizedPoints)
        {
          MarkPoint(p);
        }
      }
    }
  }
}