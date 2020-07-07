using System;

namespace Arnible.MathModeling.Export
{
  public interface IToStringSerializer
  {
    Func<object, ReadOnlyMemory<char>> Serializator { get; }
  }

  public class ToStringSerializer<T> : IToStringSerializer
  {
    private static ReadOnlyMemory<char> ReadOnlyMemorySerializator(in Func<T, string> serializator, in object obj)
    {
      if (obj is T v)
        return serializator(v).AsMemory();
      else
        throw new InvalidOperationException($"Expected instance of type {typeof(T)} got {obj?.GetType()}");
    }

    private static ReadOnlyMemory<char> ReadOnlyMemorySerializator(in Func<T, ReadOnlyMemory<char>> serializator, in object obj)
    {
      if (obj is T v)
        return serializator(v);
      else
        throw new InvalidOperationException($"Expected instance of type {typeof(T)} got {obj?.GetType()}");
    }

    public ToStringSerializer(Func<T, string> serializator)
    {
      if (serializator == null)
      {
        throw new ArgumentNullException(nameof(serializator));
      }
      Serializator = v => ReadOnlyMemorySerializator(serializator, v);
    }

    public ToStringSerializer()
      : this(v => v.ToString())
    {
      // intentionally empty
    }

    public ToStringSerializer(Func<T, ReadOnlyMemory<char>> serializator)
    {
      if (serializator == null)
      {
        throw new ArgumentNullException(nameof(serializator));
      }
      Serializator = v => ReadOnlyMemorySerializator(serializator, v);
    }

    public Func<object, ReadOnlyMemory<char>> Serializator { get; }
  }
}
