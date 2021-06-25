using System;
using Arnible.Assertions;
using Arnible.Linq.Algebra;

namespace Arnible.MathModeling.Geometry
{
  public readonly ref struct ConjugateDirection
  {
    private readonly Span2D<Number> _directions;
    private readonly SpanSingle<ushort> _directionToWriteCursor;
    private readonly SpanSingle<bool> _allBufferUsed;
    
    public ConjugateDirection(
      in Span2D<Number> directionsBuffer, 
      in SpanSingle<ushort> directionCursorBuffer,
      in SpanSingle<bool> allBufferUsed)
    {
      _directions = directionsBuffer;
      _directionToWriteCursor = directionCursorBuffer;
      _allBufferUsed = allBufferUsed;
      
      _directionToWriteCursor.Set(0);
      _allBufferUsed.Set(false);
    }

    public ushort DirectionsDimensionsCount => _directions.ColumnsCount;
    public ushort DirectionsMemorySize => _directions.RowsCount;
    
    public bool HasConjugateDirections => DirectionsMemorySize > 0 && _allBufferUsed;

    public void AddDirection(in ReadOnlySpan<Number> direction)
    {
      direction.Length.AssertIsEqualTo(DirectionsDimensionsCount);
      direction.CopyTo(_directions.Row(_directionToWriteCursor));
      if(_directionToWriteCursor + 1 < DirectionsMemorySize)
      {
        _directionToWriteCursor.Set((ushort)(_directionToWriteCursor + 1));
      }
      else
      {
        _directionToWriteCursor.Set(0);
        _allBufferUsed.Set(true);
      }
    }
    
    public ReadOnlySpan<Number> GetConjugateDirection(in Span<Number> buffer)
    {
      ushort directionsCount = _allBufferUsed ? DirectionsMemorySize : _directionToWriteCursor;
      directionsCount.AssertIsGreaterThan(0);
      for(ushort i=0; i<directionsCount; ++i)
      {
        if(i == 0)
        {
          _directions.Row(i).CopyTo(buffer);
        }
        else
        {
          buffer.AddToSelf(_directions.Row(i));
        }
      }
      return buffer[0..DirectionsDimensionsCount];
    }
  }
}