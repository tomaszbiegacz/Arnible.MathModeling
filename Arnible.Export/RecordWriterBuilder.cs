using System;
using System.Collections.Generic;
using Arnible.Export.RecordPerTextRow;

namespace Arnible.Export
{
  public interface IRecordWriterBuilder
  {
    IReferenceRecordWriter<TRecord> CreateTsvReferenceRecordWriter<TRecord>(ISimpleLogger logger) where TRecord : class?;
    IValueRecordWriter<TRecord> CreateTsvValueRecordWriter<TRecord>(ISimpleLogger logger) where TRecord : struct;
  }
  
  public class RecordWriterBuilder : IRecordWriterBuilder
  {
    private readonly Dictionary<Type, Func<Type[], object>> _serializersFactories;
    
    public RecordWriterBuilder()
    {
      _serializersFactories = new Dictionary<Type, Func<Type[], object>>();
    }
    
    public RecordWriterBuilder RegisterReferenceSerializer<TField, TRecordSerializer>()
      where TField : class
      where TRecordSerializer : class, IReferenceRecordSerializer<TField>, new()
    {
      if(!_serializersFactories.TryAdd(typeof(TField), t => new TRecordSerializer()))
      {
        throw new InvalidOperationException($"There is already serializer for {typeof(TField).FullName}");
      }
      return this;
    }
    
    public RecordWriterBuilder RegisterValueSerializer<TField, TRecordSerializer>()
      where TField : struct
      where TRecordSerializer : class, IValueRecordSerializer<TField>, new()
    {
      if(!_serializersFactories.TryAdd(typeof(TField), t => new TRecordSerializer()))
      {
        throw new InvalidOperationException($"There is already serializer for {typeof(TField).FullName}");
      }
      return this;
    }
    
    private static object CreateSerializer(Type recordSerializerOpenType, Type[] genericTypes)
    {
      Type closedType = recordSerializerOpenType.MakeGenericType(genericTypes);
      return Activator.CreateInstance(closedType) ?? throw new InvalidOperationException("CreateInstance returned null");
    }
    
    public RecordWriterBuilder RegisterGenericValueSerializer(Type genericValueOpenType, Type recordSerializerOpenType)
    {
      if(!genericValueOpenType.IsGenericTypeDefinition 
         || !genericValueOpenType.IsValueType)
      {
        throw new ArgumentException(nameof(genericValueOpenType));
      }
      if(!recordSerializerOpenType.IsGenericTypeDefinition 
         || !recordSerializerOpenType.HasParameterlessConstructor()
         || !recordSerializerOpenType.IsImplementingGenericInterface(typeof(IValueRecordSerializer<>)))
      {
        throw new ArgumentException(nameof(recordSerializerOpenType));
      }
      
      if(!_serializersFactories.TryAdd(genericValueOpenType, t => CreateSerializer(recordSerializerOpenType, t)))
      {
        throw new InvalidOperationException($"There is already serializer for {genericValueOpenType.FullName}");
      }
      return this;
    }
    
    public RecordWriterBuilder RegisterGenericReferenceSerializer(Type genericValueOpenType, Type recordSerializerOpenType)
    {
      if(!genericValueOpenType.IsGenericTypeDefinition 
         || genericValueOpenType.IsValueType)
      {
        throw new ArgumentException(nameof(genericValueOpenType));
      }
      if(!recordSerializerOpenType.IsGenericTypeDefinition 
         || !recordSerializerOpenType.HasParameterlessConstructor()
         || !recordSerializerOpenType.IsImplementingGenericInterface(typeof(IReferenceRecordSerializer<>)))
      {
        throw new ArgumentException(nameof(recordSerializerOpenType));
      }
      
      if(!_serializersFactories.TryAdd(genericValueOpenType, t => CreateSerializer(recordSerializerOpenType, t)))
      {
        throw new InvalidOperationException($"There is already serializer for {genericValueOpenType.FullName}");
      }
      return this;
    }

    public IReferenceRecordWriter<TRecord> CreateTsvReferenceRecordWriter<TRecord>(ISimpleLogger logger) where TRecord : class?
    {
      return new RecordPerRowReferenceWriter<TRecord>(
        headerFieldSubNameSeparator: TsvConst.HeaderFieldSubNameSeparator,
        rowFieldSeparator: TsvConst.RowFieldSeparator,
        _serializersFactories,
        logger);
    }
    
    public IValueRecordWriter<TRecord> CreateTsvValueRecordWriter<TRecord>(ISimpleLogger logger) where TRecord : struct
    {
      return new RecordPerRowValueWriter<TRecord>(
        headerFieldSubNameSeparator: TsvConst.HeaderFieldSubNameSeparator,
        rowFieldSeparator: TsvConst.RowFieldSeparator,
        _serializersFactories,
        logger);
    }
  }
}