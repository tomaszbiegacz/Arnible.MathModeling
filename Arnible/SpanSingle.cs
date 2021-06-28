using System;

namespace Arnible
{
#pragma warning disable 660,661
  public readonly ref struct SpanSingle<T> where T: IEquatable<T>
#pragma warning restore 660,661
  {
    private readonly Span<T> _value;
    
    public SpanSingle(in Span<T> buffer)
    {
      if(buffer.Length != 1)
        throw new ArgumentException();
      _value = buffer;
    }
    
    public ref T Value => ref _value[0];
    
    public static implicit operator T(in SpanSingle<T> src) => src.Value;
    
    public void Set(in T value)
    {
      _value[0] = value;
    }
    
    public bool Equals(in SpanSingle<T> other) => _value.SequenceEqual(other._value);
    
    public static bool operator ==(in SpanSingle<T> a, in SpanSingle<T> b) => a.Equals(in b);
    public static bool operator !=(in SpanSingle<T> a, in SpanSingle<T> b) => !a.Equals(in b);
  }
}