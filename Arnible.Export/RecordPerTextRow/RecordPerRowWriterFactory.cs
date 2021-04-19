using System;
using System.Collections.Generic;

namespace Arnible.Export.RecordPerTextRow
{
  readonly struct RecordPerRowWriterFactory
  {
    private readonly IReadOnlyDictionary<Type, Func<Type[], object>> _serializersFactories;

    public RecordPerRowWriterFactory(IReadOnlyDictionary<Type, Func<Type[], object>> serializersFactories)
    {
      _serializersFactories = serializersFactories;
    }
    
    public ICollectionFieldSerializer<TField> BuildCollectionFieldSerializer<TField>(RecordPerRowFieldSerializer parent)
    {
      Type fieldType = typeof(TField);
      Type collectionFieldSerializerType;
      if(fieldType.IsValueType)
      {
        collectionFieldSerializerType = typeof(RecordPerRowValueCollectionFieldSerializer<>)
          .MakeGenericType(fieldType);
      }
      else
      {
        collectionFieldSerializerType = typeof(RecordPerRowReferenceCollectionFieldSerializer<>)
          .MakeGenericType(fieldType);
      }
      return (ICollectionFieldSerializer<TField>)Activator.CreateInstance(collectionFieldSerializerType, parent);
    }
    
    public IReferenceRecordSerializer<TField> GetReferenceSerializer<TField>() where TField: class?
    {
      Type fieldType = typeof(TField);
      if(_serializersFactories.TryGetValue(fieldType, out Func<Type[], object> factory))
      {
        return (IReferenceRecordSerializer<TField>)factory(Type.EmptyTypes);
      }
      else if(fieldType.IsGenericType)
      {
        Type openGenericType = fieldType.GetGenericTypeDefinition();
        if(_serializersFactories.TryGetValue(openGenericType, out factory))
        {
          return (IReferenceRecordSerializer<TField>)factory(fieldType.GetGenericArguments()); 
        }
      }
      
      throw new InvalidOperationException($"Unknown serializer for type {fieldType.FullName}");
    }
    
    public IValueRecordSerializer<TField> GetValueSerializer<TField>() where TField: struct
    {
      Type fieldType = typeof(TField);
      if(_serializersFactories.TryGetValue(fieldType, out Func<Type[], object> factory))
      {
        return (IValueRecordSerializer<TField>)factory(Type.EmptyTypes);
      }
      else if(fieldType.IsGenericType)
      {
        Type openGenericType = fieldType.GetGenericTypeDefinition();
        if(_serializersFactories.TryGetValue(openGenericType, out factory))
        {
          return (IValueRecordSerializer<TField>)factory(fieldType.GetGenericArguments()); 
        }
      }
      
      throw new InvalidOperationException($"Unknown serializer for type {fieldType.FullName}");
    }
    
    public IReferenceRecordWriter<TField> BuildReferenceFieldWriter<TField>(
      RecordPerRowFieldSerializer parent,
      in ReadOnlySpan<char> fieldName) where TField: class?
    {
      return new RecordPerRowReferenceFieldWriter<TField>(parent, fieldName.ToString(), GetReferenceSerializer<TField>());
    }
    
    public IValueRecordWriter<TField> BuildValueFieldWriter<TField>(
      RecordPerRowFieldSerializer parent,
      in ReadOnlySpan<char> fieldName) where TField: struct
    {
      return new RecordPerRowValueFieldWriter<TField>(parent, fieldName.ToString(), GetValueSerializer<TField>());
    }
    
    public IRecordCollectionWriter<TField> BuildReferenceCollectionFieldWriter<TField>(
      RecordPerRowFieldSerializer parent,
      in ReadOnlySpan<char> fieldName) where TField: class?
    {
      return new RecordPerRowReferenceCollectionFieldWriter<TField>(parent, fieldName.ToString(), GetReferenceSerializer<TField>());
    }
    
    public IRecordCollectionWriter<TField> BuildValueCollectionFieldWriter<TField>(
      RecordPerRowFieldSerializer parent,
      in ReadOnlySpan<char> fieldName) where TField: struct
    {
      return new RecordPerRowValueCollectionFieldWriter<TField>(parent, fieldName.ToString(), GetValueSerializer<TField>());
    }
  }
}