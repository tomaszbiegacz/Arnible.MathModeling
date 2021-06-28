using System;

namespace Arnible
{
#pragma warning disable 660,661
  public readonly ref struct Span2D<T> where T: IEquatable<T>
#pragma warning restore 660,661
  {
    private readonly Span<T> _items;
    private readonly ushort _columnsCount;
    
    public ushort ColumnsCount => _columnsCount;
    public ushort RowsCount => _columnsCount == 0 ? (ushort)0 : (ushort)(_items.Length / _columnsCount);
    
    public Span2D(in Span<T> buffer, ushort columnsCount)
    {
      if(columnsCount == 0)
        throw new ArgumentException(nameof(columnsCount));
      if(buffer.Length % columnsCount != 0)
        throw new ArgumentException(nameof(buffer));
      _items = buffer;
      _columnsCount = columnsCount;
    }
    
    public void Clear() => _items.Clear();

    public Span<T> Row(ushort pos) => _items[(_columnsCount * pos)..(_columnsCount*(pos+1))];
    
    public bool Equals(in Span2D<T> other) => _columnsCount == other._columnsCount && _items.SequenceEqual(other._items);
    
    public static bool operator ==(in Span2D<T> a, in Span2D<T> b) => a.Equals(in b);
    public static bool operator !=(in Span2D<T> a, in Span2D<T> b) => !a.Equals(in b);
  }
}