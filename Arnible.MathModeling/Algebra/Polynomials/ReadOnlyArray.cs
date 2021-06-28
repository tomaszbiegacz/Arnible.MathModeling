using System;
using System.Collections.Generic;
using Arnible.Linq;

namespace Arnible.MathModeling.Algebra.Polynomials
{
  readonly struct ReadOnlyArray<T> where T: IEquatable<T>
  {
    private readonly static T[] _emptyArray = new T[0];
    
    private readonly T[]? _src;

    /// <summary>
    /// Allow implicit casting of arrays
    /// </summary>
    private ReadOnlyArray(T[] items)
    {
      _src = items;
    }
    
    public static implicit operator ReadOnlyArray<T>(T[] v) => new(v);
    // public static implicit operator ReadOnlySpan<T>(ReadOnlyArray<T> v) => v._src ?? _emptyArray;

    //
    // Properties
    //
    
    private IReadOnlyList<T> Src => _src ?? _emptyArray;
    
    public ushort Length => (ushort)Src.Count;

    public ref T this[int pos] => ref (_src ?? throw new InvalidOperationException())[pos];

    public override bool Equals(object? obj)
    {
      return obj is ReadOnlyArray<T> other && other.Src.SequenceEqual(Src);
    }

    public override int GetHashCode()
    {
      HashCode hashCode = new HashCode();
      foreach(T item in Src)
      {
        hashCode.Add(item);
      }
      return hashCode.ToHashCode();
    }

    

    //
    // 
    // IEquatable
    //
    
    /// <summary>
    /// Shorthand for quick and dirty code relying on linq.
    /// </summary>
    public IReadOnlyList<T>AsList() => Src;
    public IEnumerator<T> GetEnumerator() => Src.GetEnumerator();
  }
}