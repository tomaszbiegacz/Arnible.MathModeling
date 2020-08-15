using System;

namespace Arnible.MathModeling.Export
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
  public class RecordSerializerAttribute : Attribute
  {
    private readonly Func<object?, ReadOnlyMemory<char>> _serializer;

    public SerializationMediaType MediaType { get; }    
    
    public RecordSerializerAttribute(SerializationMediaType mediaType)
    {
      MediaType = mediaType;
      var objectSerializer = new ToStringSerializer<object>();
      _serializer = objectSerializer.Serializator;
    }

    public ReadOnlyMemory<char> Serialize(object? value)
    {
      return _serializer(value);
    }
  }
}
