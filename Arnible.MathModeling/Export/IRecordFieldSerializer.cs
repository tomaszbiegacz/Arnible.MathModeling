using System;
using System.Collections.Generic;

namespace Arnible.MathModeling.Export
{
  public interface IRecordFieldSerializer
  {
    bool IsSerializingFieldName { get; }

    void WriteNull(in string fieldName);
    
    void Write(in string fieldName, in byte? value);
    void Write(in string fieldName, in IReadOnlyCollection<byte>? value);
    
    void Write(in string fieldName, in sbyte? value);
    void Write(in string fieldName, in IReadOnlyCollection<sbyte>? value);
    
    void Write(in string fieldName, in short? value);
    void Write(in string fieldName, in IReadOnlyCollection<short>? value);
    
    void Write(in string fieldName, in ushort? value);
    void Write(in string fieldName, in IReadOnlyCollection<ushort>? value);
    
    void Write(in string fieldName, in uint? value);
    void Write(in string fieldName, in IReadOnlyCollection<uint>? value);
    
    void Write(in string fieldName, in int? value);
    void Write(in string fieldName, in IReadOnlyCollection<int>? value);
    
    void Write(in string fieldName, in ulong? value);
    void Write(in string fieldName, in IReadOnlyCollection<ulong>? value);
    
    void Write(in string fieldName, in long? value);
    void Write(in string fieldName, in IReadOnlyCollection<long>? value);
    
    void Write(in string fieldName, in float? value);
    void Write(in string fieldName, in IReadOnlyCollection<float>? value);
    
    void Write(in string fieldName, in double? value);
    void Write(in string fieldName, in IReadOnlyCollection<double>? value);
    
    void Write(in string fieldName, in char? value);
    void Write(in string fieldName, in IReadOnlyCollection<char>? value);
    
    void Write(in string fieldName, in string? value);
    void Write(in string fieldName, in IReadOnlyCollection<string>? value);
    
    IRecordWriter<TField> GetRecordSerializer<TField>(
      in string fieldName, 
      in Func<IRecordFieldSerializer, IRecordWriter<TField>> writerFactory);

    IRecordWriterReadOnlyCollection<TField> GetReadOnlyCollectionSerializer<TField>(
      in string fieldName,
      in Func<IRecordFieldSerializer, IRecordWriter<TField>> writerFactory);
      
    void CommitWrite();
  }
}