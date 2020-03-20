using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Arnible.MathModeling.Export
{
  class TsvFlatFieldSerializerCollection : FlatFieldSerializerCollection
  {
    static readonly Dictionary<Type, Func<object, ReadOnlyMemory<char>>> _valueSerializers;
    static readonly Dictionary<Type, TsvFlatFieldSerializerCollection> _structureSerializers;

    private static ReadOnlyMemory<char> ConvertKnown(string value)
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
      _valueSerializers = new Dictionary<Type, Func<object, ReadOnlyMemory<char>>>
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
        };
      _structureSerializers = new Dictionary<Type, TsvFlatFieldSerializerCollection>();
    }

    static Func<object, ReadOnlyMemory<char>> GetValueSerializer(Type t)
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
          _valueSerializers.Add(t, valueSerializer);
        }
      }
      return valueSerializer;
    }

    static TsvFlatFieldSerializerCollection GetStructureSerializer(Type t)
    {
      if (!_structureSerializers.TryGetValue(t, out var fields))
      {
        fields = new TsvFlatFieldSerializerCollection(t);
        _structureSerializers.Add(t, fields);
      }
      return fields;
    }

    public static IFlatFieldSerializerCollection For<T>() => GetStructureSerializer(typeof(T));

    static FlatFieldSerializer ForRootProperty(PropertyInfo property, Func<object, ReadOnlyMemory<char>> valueSerializer)
    {
      return new FlatFieldSerializer(property, valueSerializer);
    }

    static bool IsEnumerable(Type t)
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
      if(fixedArray == null)
      {
        throw new InvalidOperationException("Only fixed sized enumerables are supported.");
      }      

      Type enumerableInterfaceType = property
        .PropertyType.GetInterfaces()
        .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
      Type elementType = enumerableInterfaceType.GetGenericArguments().Single();
      TsvFlatFieldSerializerCollection structureSerializer = GetStructureSerializer(elementType);

      for(uint i=0; i<fixedArray.Size; ++i)
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
        return ForRootProperty(property, valueSerializer).Yield();
      }
      else
      {
        if (IsEnumerable(t))
          return ResolveEnumerableStructures(property);
        else
          return ResolveStructureProperties(property);
      }
    }

    private TsvFlatFieldSerializerCollection(Type t)
      : base(t, GetProperties(t).SelectMany(p => ForRootProperty(p)))
    {
      // intentionally empty
    }
  }
}
