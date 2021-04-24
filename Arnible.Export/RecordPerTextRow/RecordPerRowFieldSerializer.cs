using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Arnible.Export.RecordPerTextRow
{
  class RecordPerRowFieldSerializer : IRecordFieldSerializer
  {
    private readonly RecordPerRowStream _stream;
    private readonly RecordPerRowWriterFactory _writersFactory;
    protected readonly NamespaceWithName _fieldNamespace;
    
    private readonly List<object> _writersCache;
    private ushort _currentWriter;

    protected RecordPerRowFieldSerializer(
      RecordPerRowFieldSerializer parent,
      string fieldName)
    {
      _writersCache = new List<object>();
      _currentWriter = 0;
      
      _stream = parent._stream;
      _writersFactory = parent._writersFactory;
      _fieldNamespace = parent._fieldNamespace.SubName(fieldName);
    }
    
    public RecordPerRowFieldSerializer(
      string headerFieldSubNameSeparator,
      string rowFieldSeparator,
      IReadOnlyDictionary<Type, Func<Type[], object>> serializersFactories,
      ISimpleLogger logger)
    {
      _writersCache = new List<object>();
      _currentWriter = 0;
      
      _stream = new RecordPerRowStream(rowFieldSeparator, logger);
      _writersFactory = new RecordPerRowWriterFactory(serializersFactories);
      _fieldNamespace = new NamespaceWithName(headerFieldSubNameSeparator);
    }

    protected NamespaceWithName? FieldNamespace
    {
      get
      {
        if(_stream .IsHeaderSerialized)
        {
          return null;
        }
        else
        {
          return _fieldNamespace;
        }
      }
    }

    public void BeginRecord()
    {
      _currentWriter = 0;
    }
    
    protected void CommitStream()
    {
      _stream.CommitRecord();
    }
    
    protected IReferenceRecordSerializer<TField> GetReferenceSerializer<TField>() where TField: class?
    {
      return _writersFactory.GetReferenceSerializer<TField>();
    }
    
    protected IValueRecordSerializer<TField> GetValueSerializer<TField>() where TField: struct
    {
      return _writersFactory.GetValueSerializer<TField>();
    }
    
    //
    // Write
    //
    
    public void WriteNull(in ReadOnlySpan<char> fieldName)
    {
      _stream.WriteNull(FieldNamespace, in fieldName);
    }

    public void Write(in ReadOnlySpan<char> fieldName, byte value)
    {
      Write(in fieldName, (int)value);
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, byte? value)
    {
      Write(in fieldName, (int?)value);
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, sbyte value)
    {
      Write(in fieldName, (int)value);
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, sbyte? value)
    {
      Write(in fieldName, (int?)value);
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, ushort value)
    {
      Write(in fieldName, (int)value);
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, ushort? value)
    {
      Write(in fieldName, (int?)value);
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, short value)
    {
      Write(in fieldName, (int)value);
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, short? value)
    {
      Write(in fieldName, (int?)value);
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, int? value)
    {
      if(value is null)
      {
        _stream.WriteNull(FieldNamespace, in fieldName);
      }
      else
      {
        Write(in fieldName, value.Value);
      }
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, int value)
    {
      Span<char> buffer = stackalloc char[SpanCharFormatter.BufferSize];
      _stream.WriteValue(
        FieldNamespace, 
        in fieldName, 
        SpanCharFormatter.ToString(value, buffer));
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, uint? value)
    {
      if(value is null)
      {
        _stream.WriteNull(FieldNamespace, in fieldName);
      }
      else
      {
        Write(in fieldName, value.Value);
      }
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, uint value)
    {
      Span<char> buffer = stackalloc char[SpanCharFormatter.BufferSize];
      _stream.WriteValue(
        FieldNamespace, 
        in fieldName, 
        SpanCharFormatter.ToString(value, buffer));
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, in long? value)
    {
      if(value is null)
      {
        _stream.WriteNull(FieldNamespace, in fieldName);
      }
      else
      {
        Write(in fieldName, value.Value);
      }
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, in long value)
    {
      Span<char> buffer = stackalloc char[SpanCharFormatter.BufferSize];
      _stream.WriteValue(
        FieldNamespace, 
        in fieldName, 
        SpanCharFormatter.ToString(in value, buffer));
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, in ulong? value)
    {
      if(value is null)
      {
        _stream.WriteNull(FieldNamespace, in fieldName);
      }
      else
      {
        Write(in fieldName, value.Value);
      }
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, in ulong value)
    {
      Span<char> buffer = stackalloc char[SpanCharFormatter.BufferSize];
      _stream.WriteValue(
        FieldNamespace, 
        in fieldName, 
        SpanCharFormatter.ToString(in value, buffer));
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, in float? value)
    {
      if(value is null)
      {
        _stream.WriteNull(FieldNamespace, in fieldName);
      }
      else
      {
        Span<char> buffer = stackalloc char[SpanCharFormatter.BufferSize];
        _stream.WriteValue(
          FieldNamespace, 
          in fieldName, 
          SpanCharFormatter.ToString(value.Value, buffer));
      }
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, in double? value)
    {
      if(value is null)
      {
        _stream.WriteNull(FieldNamespace, in fieldName);
      }
      else
      {
        Write(in fieldName, value.Value);
      }
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, in double value)
    {
      Span<char> buffer = stackalloc char[SpanCharFormatter.BufferSize];
      _stream.WriteValue(
        FieldNamespace, 
        in fieldName, 
        SpanCharFormatter.ToString(in value, buffer));
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, in decimal? value)
    {
      if(value is null)
      {
        _stream.WriteNull(FieldNamespace, in fieldName);
      }
      else
      {
        Write(in fieldName, value.Value);
      }
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, in decimal value)
    {
      Span<char> buffer = stackalloc char[SpanCharFormatter.BufferSize];
      _stream.WriteValue(
        FieldNamespace, 
        in fieldName, 
        SpanCharFormatter.ToString(in value, buffer));
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, char? value)
    {
      if(value is null)
      {
        _stream.WriteNull(FieldNamespace, in fieldName);
      }
      else
      {
        ReadOnlySpan<char> serialized = stackalloc char[] { value.Value };
        _stream.WriteValue(FieldNamespace, in fieldName, in serialized);
      }
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, string? value)
    {
      if(string.IsNullOrWhiteSpace(value))
      {
        _stream.WriteNull(FieldNamespace, in fieldName);
      }
      else
      {
        _stream.WriteValue(FieldNamespace, in fieldName, value);
      }
    }
    
    public void Write(in ReadOnlySpan<char> fieldName, in ReadOnlySpan<char> value)
    {
      _stream.WriteValue(FieldNamespace, in fieldName, in value);
    }
    
    //
    // Fields writers 
    //
    
    private bool NextRecordWriter<T>([MaybeNullWhen(false)] out T? result) where T: class
    {
      if(_currentWriter == _writersCache.Count)
      {
        result = null;
        return false;
      }
      else
      {
        result = (T)_writersCache[_currentWriter];
        _currentWriter++;
        return true;
      }
    }
    
    private void AddRecordWriter<T>(T result) where T: class
    {
      _writersCache.Add(result);
      _currentWriter++;
    }
    
    private IReferenceRecordWriter<TField> GetReferenceFieldWriter<TField>(in ReadOnlySpan<char> fieldName) where TField: class?
    {
      IReferenceRecordWriter<TField>? result;
      if(!NextRecordWriter(out result))
      {
        result = _writersFactory.BuildReferenceFieldWriter<TField>(this, in fieldName);
        AddRecordWriter(result);
      }
      return result!;
    }

    private IValueRecordWriter<TField> GetValueFieldWriter<TField>(in ReadOnlySpan<char> fieldName) where TField: struct
    {
      IValueRecordWriter<TField>? result;
      if(!NextRecordWriter(out result))
      {
        result = _writersFactory.BuildValueFieldWriter<TField>(this, in fieldName);
        AddRecordWriter(result);
      }
      return result!;
    }

    public void WriteReferenceField<TField>(in ReadOnlySpan<char> fieldName, TField field) where TField : class?
    {
      GetReferenceFieldWriter<TField>(in fieldName).Write(field);
    }
    
    public void WriteValueField<TField>(in ReadOnlySpan<char> fieldName, in TField field) where TField : struct
    {
      GetValueFieldWriter<TField>(in fieldName).Write(in field);
    }

    public void WriteValueField<TField>(in ReadOnlySpan<char> fieldName, in TField? field) where TField : struct
    {
      GetValueFieldWriter<TField>(in fieldName).Write(in field);
    }
    
    public IRecordCollectionWriter<TField> GetReferenceRecordCollectionWriter<TField>(in ReadOnlySpan<char> fieldName) where TField: class?
    {
      IRecordCollectionWriter<TField>? result;
      if(!NextRecordWriter(out result))
      {
        result = _writersFactory.BuildReferenceCollectionFieldWriter<TField>(this, in fieldName);
        AddRecordWriter(result);
      }
      return result!;
    }
    
    public IRecordCollectionWriter<TField> GetValueRecordCollectionWriter<TField>(in ReadOnlySpan<char> fieldName) where TField: struct
    {
      IRecordCollectionWriter<TField>? result;
      if(!NextRecordWriter(out result))
      {
        result = _writersFactory.BuildValueCollectionFieldWriter<TField>(this, in fieldName);
        AddRecordWriter(result);
      }
      return result!;
    }
    
    public ICollectionFieldSerializer<TField> CollectionField<TField>()
    {
      ICollectionFieldSerializer<TField>? result;
      if(!NextRecordWriter(out result))
      {
        result = _writersFactory.BuildCollectionFieldSerializer<TField>(this);
        AddRecordWriter(result);
      }
      
      return result!;
    }
  }
}