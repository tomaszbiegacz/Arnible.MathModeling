using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Arnible.MathModeling.Export
{
  interface IFlatFieldSerializer
  {
    ReadOnlyMemory<char> Header { get; }

    Func<object, TextWriter, CancellationToken, Task> Serializer { get; }
  }

  class FlatFieldSerializer : IFlatFieldSerializer
  {
    static string GetHeaderNameWithPrefix(string name, string prefix) => prefix == null ? name : $"{prefix}_{name}";

    static Func<object, ReadOnlyMemory<char>> NullablePropertySerializer(PropertyInfo property, Func<object, ReadOnlyMemory<char>> valueSerializer)
    {
      return o => o == null ? default : valueSerializer(property.GetValue(o));
    }

    private readonly Func<object, ReadOnlyMemory<char>> _valueSerializer;

    public FlatFieldSerializer(PropertyInfo property, Func<object, ReadOnlyMemory<char>> valueSerializer)
      : this(property.Name, NullablePropertySerializer(property, valueSerializer))
    {
      // intentionally empty
    }

    private FlatFieldSerializer(string header, Func<object, ReadOnlyMemory<char>> valueSerializer)
    {
      Header = header.AsMemory();
      _valueSerializer = valueSerializer ?? throw new ArgumentNullException(nameof(valueSerializer));
      Serializer = (value, stream, token) => stream.WriteAsync(_valueSerializer(value), token);
    }

    public ReadOnlyMemory<char> Header { get; }

    public Func<object, TextWriter, CancellationToken, Task> Serializer { get; }

    public FlatFieldSerializer ForProperty(PropertyInfo property)
    {
      return new FlatFieldSerializer(
        GetHeaderNameWithPrefix(new string(Header.Span), property.Name),
        NullablePropertySerializer(property, _valueSerializer));
    }
  }
}
