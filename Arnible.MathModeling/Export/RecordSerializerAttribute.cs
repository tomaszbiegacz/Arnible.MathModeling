using System;

namespace Arnible.MathModeling.Export
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
  public class RecordSerializerAttribute : Attribute
  {
    private readonly Func<object, ReadOnlyMemory<char>> _serializator;

    public SerializationMediaType MediaType { get; }

    public bool HasStructure => _serializator == null;

    public RecordSerializerAttribute(SerializationMediaType mediaType, Func<object, ReadOnlyMemory<char>> serializator)
    {
      MediaType = mediaType;
      _serializator = serializator ?? throw new ArgumentNullException(nameof(serializator));
    }

    public RecordSerializerAttribute(SerializationMediaType mediaType, IToStringSerializer serializator)
    {
      MediaType = mediaType;
      _serializator = serializator?.Serializator ?? throw new ArgumentNullException(nameof(serializator));
    }

    public RecordSerializerAttribute(SerializationMediaType mediaType, Type serializatorType)
    {
      MediaType = mediaType;
      object serializator = Activator.CreateInstance(serializatorType);
      if (serializator is IToStringSerializer typedSerializer)
      {
        _serializator = typedSerializer.Serializator;
      }
      else
      {
        throw new ArgumentException("Serializer needs to implement IToStringSerializer interface.");
      }
    }

    public RecordSerializerAttribute(SerializationMediaType mediaType)
    {
      MediaType = mediaType;
      _serializator = null;
    }

    public ReadOnlyMemory<char> Serialize(object value)
    {
      if (HasStructure)
      {
        throw new InvalidOperationException("Serialize method can be only called for simplified serialization.");
      }

      return _serializator(value);
    }
  }
}
