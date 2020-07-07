using System;
using System.Collections;
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
    static string GetHeaderNameWithPrefix(string prefix, string name) => prefix == null ? name : $"{prefix}_{name}";

    static Func<object, ReadOnlyMemory<char>> NullablePropertySerializer(
      PropertyInfo property,
      Func<object, ReadOnlyMemory<char>> valueSerializer)
    {
      return o => o == null ? default : valueSerializer(property.GetValue(o));
    }

    static string GetHeaderNameWithPrefix(string prefix, uint pos, string name) => $"{prefix}_{pos}_{name}";

    static object GetIndexedPropertyValue(
      in PropertyInfo property, 
      in uint pos, 
      in object value)
    {
      object propertyValue = property.GetValue(value);
      if (propertyValue == null)
      {
        return null;
      }

      var enumValue = (IEnumerable)propertyValue;
      IEnumerator enumerator = enumValue.GetEnumerator();

      uint i = 0;
      while (enumerator.MoveNext())
      {
        if (i == pos)
        {
          return enumerator.Current;
        }
        else
        {
          i++;
        }
      }

      return null;
    }

    static Func<object, ReadOnlyMemory<char>> NullablePropertySerializer(
      PropertyInfo property,
      uint pos,
      Func<object, ReadOnlyMemory<char>> valueSerializer)
    {
      return o => o == null ? default : valueSerializer(GetIndexedPropertyValue(property, pos, o));
    }

    private readonly Func<object, ReadOnlyMemory<char>> _valueSerializer;

    public FlatFieldSerializer(
      in PropertyInfo property, 
      in Func<object, ReadOnlyMemory<char>> valueSerializer)
      : this(property.Name, NullablePropertySerializer(property, valueSerializer))
    {
      // intentionally empty
    }

    private FlatFieldSerializer(in string header, in Func<object, ReadOnlyMemory<char>> valueSerializer)
    {
      Header = header.AsMemory();
      _valueSerializer = valueSerializer ?? throw new ArgumentNullException(nameof(valueSerializer));
      Serializer = (value, stream, token) => stream.WriteAsync(_valueSerializer(value), token);
    }

    public ReadOnlyMemory<char> Header { get; }

    public Func<object, TextWriter, CancellationToken, Task> Serializer { get; }

    public FlatFieldSerializer ForProperty(in PropertyInfo property)
    {
      return new FlatFieldSerializer(
        GetHeaderNameWithPrefix(property.Name, new string(Header.Span)),
        NullablePropertySerializer(property, _valueSerializer));
    }

    public FlatFieldSerializer ForIndexedProperty(in PropertyInfo property, in uint pos)
    {
      return new FlatFieldSerializer(
        GetHeaderNameWithPrefix(property.Name, pos, new string(Header.Span)),
        NullablePropertySerializer(property, pos, _valueSerializer));
    }
  }
}
