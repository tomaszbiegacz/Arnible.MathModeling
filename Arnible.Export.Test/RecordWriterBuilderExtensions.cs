namespace Arnible.Export.Test
{
  public static class RecordWriterBuilderExtensions
  {
    public static RecordWriterBuilder RegisterTestSerializers(this RecordWriterBuilder src)
    {
      return src
        .RegisterCoreSerializers()
        .RegisterMathModellingSerializers()
        
        .RegisterReferenceSerializer<NullableSubRecord, NullableSubRecord.Serializer>()
        .RegisterReferenceSerializer<TestReferenceRecord, TestReferenceRecord.Serializer>()
        .RegisterReferenceSerializer<TestSubReferenceRecord, TestSubReferenceRecord.Serializer>()
        .RegisterReferenceSerializer<TestArraysReferenceRecord, TestArraysReferenceRecord.Serializer>()
        .RegisterGenericReferenceSerializer(typeof(ReadOnlyArrayWrapper<>), typeof(ReadOnlyArrayWrapperSerializer<>))
        
        .RegisterValueSerializer<TestValueRecord, TestValueRecord.Serializer>()
        .RegisterValueSerializer<TestReferenceRecordArray, TestReferenceRecordArray.Serializer>()
        .RegisterValueSerializer<TestSubValueRecord, TestSubValueRecord.Serializer>()
        .RegisterValueSerializer<ValueRecord, ValueRecord.Serializer>();
    }
  }
}