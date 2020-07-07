using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Arnible.MathModeling.Export
{
  class TsvFlatFieldSerializerCollection : FlatFieldSerializerCollection
  {
    static readonly ConcurrentDictionary<Type, Func<object, ReadOnlyMemory<char>>> _valueSerializers;
    static readonly ConcurrentDictionary<Type, TsvFlatFieldSerializerCollection> _structureSerializers;

    private static ReadOnlyMemory<char> ConvertKnown(in string value)
    {
      if (string.IsNullOrWhiteSpace(value))
        return default;

      string trimValue = value.Trim();
      if (trimValue.Contains(TsvConst.SeparatorChar))
      {
        throw new ArgumentException($"Value [{value}] contains tab!");
      }
      return trimValue.AsMemory();
    }

    static TsvFlatFieldSerializerCollection()
    {
      _valueSerializers = new ConcurrentDictionary<Type, Func<object, ReadOnlyMemory<char>>>(new Dictionary<Type, Func<object, ReadOnlyMemory<char>>>
        {
          { typeof(byte),     new ToStringSerializer<byte>(v => v.ToString(CultureInfo.InvariantCulture)).Serializator },
          { typeof(sbyte),    new ToStringSerializer<sbyte>(v => v.ToString(CultureInfo.InvariantCulture)).Serializator },
          { typeof(short),    new ToStringSerializer<short>(v => v.ToString(CultureInfo.InvariantCulture)).Serializator },
          { typeof(ushort),   new ToStringSerializer<ushort>(v => v.ToString(CultureInfo.InvariantCulture)).Serializator },
          { typeof(int),      new ToStringSerializer<int>(v => v.ToString(CultureInfo.InvariantCulture)).Serializator },
          { typeof(uint),     new ToStringSerializer<uint>(v => v.ToString(CultureInfo.InvariantCulture)).Serializator },
          { typeof(long),     new ToStringSerializer<long>(v => v.ToString(CultureInfo.InvariantCulture)).Serializator },
          { typeof(ulong),    new ToStringSerializer<ulong>(v => v.ToString(CultureInfo.InvariantCulture)).Serializator },
          { typeof(double),   new ToStringSerializer<double>(v => v.ToString(CultureInfo.InvariantCulture)).Serializator },
          { typeof(char),     new ToStringSerializer<char>(v => new ReadOnlyMemory<char>(new[] { v })).Serializator },
          { typeof(string),   new ToStringSerializer<string>(v => ConvertKnown(v)).Serializator }
        });
      _structureSerializers = new ConcurrentDictionary<Type, TsvFlatFieldSerializerCollection>();
    }

    static Func<object, ReadOnlyMemory<char>> GetValueSerializer(in Type t)
    {
      Func<object, ReadOnlyMemory<char>> valueSerializer = null;
      if (_valueSerializers.TryGetValue(t, out var serializator))
      {
        valueSerializer = serializator;
      }
      else
      {
        var serializatorAttr = t
          .GetCustomAttributes()
          .OfType<RecordSerializerAttribute>()
          .Where(a => a.MediaType == SerializationMediaType.TabSeparatedValues)
          .SingleOrDefault();

        if (serializatorAttr != null)
        {
          valueSerializer = serializatorAttr.Serialize;
          _valueSerializers.TryAdd(t, valueSerializer);
        }
      }
      return valueSerializer;
    }

    static TsvFlatFieldSerializerCollection GetStructureSerializer(in Type t)
    {
      if (!_structureSerializers.TryGetValue(t, out var fields))
      {
        fields = new TsvFlatFieldSerializerCollection(in t);
        _structureSerializers.TryAdd(t, fields);
      }
      return fields;
    }

    public static IFlatFieldSerializerCollection For<T>() => GetStructureSerializer(typeof(T));

    static FlatFieldSerializer ForRootProperty(in PropertyInfo property, in Func<object, ReadOnlyMemory<char>> valueSerializer)
    {
      return new FlatFieldSerializer(in property, in valueSerializer);
    }

    static bool IsEnumerable(in Type t)
    {
      return t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
    }

    static IEnumerable<FlatFieldSerializer> ResolveStructureProperties(PropertyInfo property)
    {
      TsvFlatFieldSerializerCollection structureSerializer = GetStructureSerializer(property.PropertyType);
      foreach (FlatFieldSerializer field in structureSerializer._fields)
      {
        yield return field.ForProperty(property);
      }
    }

    static IEnumerable<FlatFieldSerializer> ResolveEnumerableStructures(PropertyInfo property)
    {
      FixedArraySerializerAttribute fixedArray = property.GetCustomAttribute<FixedArraySerializerAttribute>();
      if (fixedArray == null)
      {
        throw new InvalidOperationException("Only fixed sized enumerables are supported.");
      }

      Type enumerableInterfaceType = property
        .PropertyType.GetInterfaces()
        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        .Single();
      Type elementType = enumerableInterfaceType.GetGenericArguments().Single();
      TsvFlatFieldSerializerCollection structureSerializer = GetStructureSerializer(elementType);

      for (uint i = 0; i < fixedArray.Size; ++i)
      {
        foreach (FlatFieldSerializer field in structureSerializer._fields)
        {
          yield return field.ForIndexedProperty(property, i);
        }
      }
    }

    static IEnumerable<FlatFieldSerializer> ForRootProperty(PropertyInfo property)
    {
      Type t = property.PropertyType;

      var valueSerializer = GetValueSerializer(t);
      if (valueSerializer != null)
      {
        return LinqEnumerable.Yield(ForRootProperty(property, valueSerializer));
      }
      else
      {
        if (IsEnumerable(t))
          return ResolveEnumerableStructures(property);
        else
          return ResolveStructureProperties(property);
      }
    }

    private TsvFlatFieldSerializerCollection(in Type t)
      : base(in t, GetProperties(in t).SelectMany(p => ForRootProperty(p)))
    {
      // intentionally empty
    }
  }
}
