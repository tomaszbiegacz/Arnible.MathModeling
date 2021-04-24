using System;

namespace Arnible.Export
{
  /// <summary>
  /// Low level record serialisation API
  /// </summary>
  public interface IRecordFieldSerializer
  {
    void WriteNull(in ReadOnlySpan<char> fieldName);
    
    
    void Write(in ReadOnlySpan<char> fieldName, byte value);
    void Write(in ReadOnlySpan<char> fieldName, byte? value);
    void Write(in ReadOnlySpan<char> fieldName, sbyte value);
    void Write(in ReadOnlySpan<char> fieldName, sbyte? value);
    void Write(in ReadOnlySpan<char> fieldName, ushort value);
    void Write(in ReadOnlySpan<char> fieldName, ushort? value);
    void Write(in ReadOnlySpan<char> fieldName, short value);
    void Write(in ReadOnlySpan<char> fieldName, short? value);
    void Write(in ReadOnlySpan<char> fieldName, int value);
    void Write(in ReadOnlySpan<char> fieldName, int? value);
    void Write(in ReadOnlySpan<char> fieldName, uint value);
    void Write(in ReadOnlySpan<char> fieldName, uint? value);
    void Write(in ReadOnlySpan<char> fieldName, in long? value);
    void Write(in ReadOnlySpan<char> fieldName, in long value);
    void Write(in ReadOnlySpan<char> fieldName, in ulong? value);
    void Write(in ReadOnlySpan<char> fieldName, in ulong value);
    void Write(in ReadOnlySpan<char> fieldName, in float? value);
    void Write(in ReadOnlySpan<char> fieldName, in double? value);
    void Write(in ReadOnlySpan<char> fieldName, in double value);
    void Write(in ReadOnlySpan<char> fieldName, in decimal? value);
    void Write(in ReadOnlySpan<char> fieldName, in decimal value);
    
    
    void Write(in ReadOnlySpan<char> fieldName, char? value);
    void Write(in ReadOnlySpan<char> fieldName, string? value);
    void Write(in ReadOnlySpan<char> fieldName, in ReadOnlySpan<char> value);
    
    
    void WriteReferenceField<TField>(in ReadOnlySpan<char> fieldName, TField field) where TField: class?;
    
    void WriteValueField<TField>(in ReadOnlySpan<char> fieldName, in TField? field) where TField: struct;
    void WriteValueField<TField>(in ReadOnlySpan<char> fieldName, in TField field) where TField : struct;
    
    
    ICollectionFieldSerializer<TField> CollectionField<TField>();
  }
}