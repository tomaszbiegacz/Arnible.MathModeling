using System;

namespace Arnible.MathModeling.Export
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
  public class RecordSerializerAttribute : Attribute
  {
    private readonly Func<object, ReadOnlyMemory<char>> _serializator;

    public SerializationMediaType MediaType { get; }    

    public RecordSerializerAttribute(
      SerializationMediaType mediaType, 
      Type serializatorType)
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
      var objectSerializator = new ToStringSerializer<object>();
      _serializator = objectSerializator.Serializator;
    }

    public ReadOnlyMemory<char> Serialize(object value)
    {
      return _serializator(value);
    }
  }
}
