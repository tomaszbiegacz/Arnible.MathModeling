using System;
using System.Collections.Generic;

namespace Arnible.MathModeling.Export
{
  class RecordPerRowSerializerField : IRecordFieldSerializer
  {
    private readonly IMathModelingLogger _logger;
    
    private readonly RowStringBuilder _headerBuilder;
    private readonly char _headerPartsSeparator;
    private bool _isHeaderSerialized;
    
    private readonly RowStringBuilder _rowBuilder;

    public RecordPerRowSerializerField(
      IMathModelingLogger logger,
      char headerPartsSeparator,
      char fieldSeparator)
    {
      _logger = logger;
      HeaderPartsSeparator = headerPartsSeparator;
      
      _headerBuilder = new RowStringBuilder(fieldSeparator);
      _headerPartsSeparator = headerPartsSeparator;
      _isHeaderSerialized = false;
      
      _rowBuilder = new RowStringBuilder(fieldSeparator);
    }
    
    public char HeaderPartsSeparator { get; }    
    public bool IsSerializingFieldName => !_isHeaderSerialized;

    public void CommitWrite()
    {
      if (!_isHeaderSerialized)
      {
        _logger.Log(_headerBuilder.Flush());
        _isHeaderSerialized = true;

      }
      _logger.Log(_rowBuilder.Flush());
    }

    //
    // Write
    //

    private void WriteField(in string fieldName, in string value)
    {
      if (IsSerializingFieldName)
      {
        _headerBuilder.Add(in fieldName);  
      }
      _rowBuilder.Add(in value);
    }
    
    private void WriteField(in string fieldName, in IEnumerable<string>? values)
    {
      if (values != null)
      {
        uint count = 0;
        foreach (string value in values)
        {
          count++;
          _rowBuilder.Add(in value);
        }
        
        if (IsSerializingFieldName)
        {
          for(uint i=0; i<count; ++i)
          {
            _headerBuilder.Add(in fieldName);
            _headerBuilder.Append(in _headerPartsSeparator);
            _headerBuilder.Append(in i);
          }  
        }
      }
    }

    public void WriteNull(in string fieldName)
    {
      WriteField(in fieldName, string.Empty);
    }

    public void Write(in string fieldName, in byte? value)
    {
      WriteField(in fieldName, BuiltinSerializer.AsString(in value));
    }

    public void Write(in string fieldName, in IReadOnlyCollection<byte>? value)
    {
      WriteField(in fieldName, value?.Select(v => BuiltinSerializer.AsString(v)));
    }

    public void Write(in string fieldName, in sbyte? value)
    {
      WriteField(in fieldName, BuiltinSerializer.AsString(in value));
    }
    
    public void Write(in string fieldName, in IReadOnlyCollection<sbyte>? value)
    {
      WriteField(in fieldName, value?.Select(v => BuiltinSerializer.AsString(v)));
    }

    public void Write(in string fieldName, in short? value)
    {
      WriteField(in fieldName, BuiltinSerializer.AsString(in value));
    }
    
    public void Write(in string fieldName, in IReadOnlyCollection<short>? value)
    {
      WriteField(in fieldName, value?.Select(v => BuiltinSerializer.AsString(v)));
    }
    
    public void Write(in string fieldName, in ushort? value)
    {
      WriteField(in fieldName, BuiltinSerializer.AsString(in value));
    }
    
    public void Write(in string fieldName, in IReadOnlyCollection<ushort>? value)
    {
      WriteField(in fieldName, value?.Select(v => BuiltinSerializer.AsString(v)));
    }

    public void Write(in string fieldName, in uint? value)
    {
      WriteField(in fieldName, BuiltinSerializer.AsString(in value));
    }
    
    public void Write(in string fieldName, in IReadOnlyCollection<uint>? value)
    {
      WriteField(in fieldName, value?.Select(v => BuiltinSerializer.AsString(v)));
    }

    public void Write(in string fieldName, in int? value)
    {
      WriteField(in fieldName, BuiltinSerializer.AsString(in value));
    }
    
    public void Write(in string fieldName, in IReadOnlyCollection<int>? value)
    {
      WriteField(in fieldName, value?.Select(v => BuiltinSerializer.AsString(v)));
    }

    public void Write(in string fieldName, in ulong? value)
    {
      WriteField(in fieldName, BuiltinSerializer.AsString(in value));
    }
    
    public void Write(in string fieldName, in IReadOnlyCollection<ulong>? value)
    {
      WriteField(in fieldName, value?.Select(v => BuiltinSerializer.AsString(v)));
    }

    public void Write(in string fieldName, in long? value)
    {
      WriteField(in fieldName, BuiltinSerializer.AsString(in value));
    }
    
    public void Write(in string fieldName, in IReadOnlyCollection<long>? value)
    {
      WriteField(in fieldName, value?.Select(v => BuiltinSerializer.AsString(v)));
    }

    public void Write(in string fieldName, in float? value)
    {
      WriteField(in fieldName, BuiltinSerializer.AsString(in value));
    }
    
    public void Write(in string fieldName, in IReadOnlyCollection<float>? value)
    {
      WriteField(in fieldName, value?.Select(v => BuiltinSerializer.AsString(v)));
    }

    public void Write(in string fieldName, in double? value)
    {
      WriteField(in fieldName, BuiltinSerializer.AsString(in value));
    }
    
    public void Write(in string fieldName, in IReadOnlyCollection<double>? value)
    {
      WriteField(in fieldName, value?.Select(v => BuiltinSerializer.AsString(v)));
    }

    public void Write(in string fieldName, in char? value)
    {
      WriteField(in fieldName, BuiltinSerializer.AsString(in value));
    }
    
    public void Write(in string fieldName, in IReadOnlyCollection<char>? value)
    {
      WriteField(in fieldName, value?.Select(v => BuiltinSerializer.AsString(v)));
    }

    public void Write(in string fieldName, in string? value)
    {
      WriteField(in fieldName, BuiltinSerializer.AsString(in value));
    }
    
    public void Write(in string fieldName, in IReadOnlyCollection<string>? value)
    {
      WriteField(in fieldName, value);
    }
    
    public IRecordWriter<TField> GetRecordSerializer<TField>(
      in string fieldName, 
      in Func<IRecordFieldSerializer, IRecordWriter<TField>> writerFactory)
    {
      return new RecordPerRowSerializerRecord<TField>(this, in fieldName, in writerFactory);
    }
    
    public IRecordWriterReadOnlyCollection<TField> GetReadOnlyCollectionSerializer<TField>(
      in string fieldName,
      in Func<IRecordFieldSerializer, IRecordWriter<TField>> writerFactory)
    {
      return new RecordPerRowSerializerReadOnlyCollection<TField>(this, in fieldName, in writerFactory);
    }
  }
}