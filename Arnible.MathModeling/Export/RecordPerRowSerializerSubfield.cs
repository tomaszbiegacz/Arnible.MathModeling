using System;
using System.Collections.Generic;

namespace Arnible.MathModeling.Export
{
  abstract class RecordPerRowSerializerSubfield : IRecordFieldSerializer
  {
    private readonly RecordPerRowSerializerField _fieldSerializer;

    protected RecordPerRowSerializerSubfield(in RecordPerRowSerializerField fieldSerializer)
    {
      _fieldSerializer = fieldSerializer;
    }
    
    protected abstract string GetFieldName(in string fieldName);
    
    //
    // IRecordFieldSerializer
    //

    public bool IsSerializingFieldName => _fieldSerializer.IsSerializingFieldName;

    public void WriteNull(in string fieldName)
    {
      _fieldSerializer.WriteNull(GetFieldName(in fieldName));
    }
    
    public void Write(in string fieldName, in byte? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public void Write(in string fieldName, in IReadOnlyCollection<byte>? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public void Write(in string fieldName, in sbyte? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public void Write(in string fieldName, in IReadOnlyCollection<sbyte>? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public void Write(in string fieldName, in short? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public void Write(in string fieldName, in IReadOnlyCollection<short>? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }
    
    public void Write(in string fieldName, in ushort? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public void Write(in string fieldName, in IReadOnlyCollection<ushort>? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public void Write(in string fieldName, in uint? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public void Write(in string fieldName, in IReadOnlyCollection<uint>? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public void Write(in string fieldName, in int? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public void Write(in string fieldName, in IReadOnlyCollection<int>? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public void Write(in string fieldName, in ulong? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public void Write(in string fieldName, in IReadOnlyCollection<ulong>? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public void Write(in string fieldName, in long? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public void Write(in string fieldName, in IReadOnlyCollection<long>? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public void Write(in string fieldName, in float? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public void Write(in string fieldName, in IReadOnlyCollection<float>? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public void Write(in string fieldName, in double? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public void Write(in string fieldName, in IReadOnlyCollection<double>? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public void Write(in string fieldName, in char? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public void Write(in string fieldName, in IReadOnlyCollection<char>? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public void Write(in string fieldName, in string? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public void Write(in string fieldName, in IReadOnlyCollection<string>? value)
    {
      _fieldSerializer.Write(GetFieldName(in fieldName), in value);
    }

    public IRecordWriter<TField> GetRecordSerializer<TField>(
      in string fieldName, 
      in Func<IRecordFieldSerializer, IRecordWriter<TField>> writerFactory)
    {
      return _fieldSerializer.GetRecordSerializer(GetFieldName(in fieldName), in writerFactory);
    }

    public IRecordWriterReadOnlyCollection<TField> GetReadOnlyCollectionSerializer<TField>(
      in string fieldName, 
      in Func<IRecordFieldSerializer, IRecordWriter<TField>> writerFactory)
    {
      return _fieldSerializer.GetReadOnlyCollectionSerializer(GetFieldName(in fieldName), in writerFactory);
    }

    public void CommitWrite()
    {
      // intentionally empty
    }
  }
}