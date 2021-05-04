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
        .RegisterValueSerializer<TestValueRecord, TestValueRecord.Serializer>()
        .RegisterReferenceSerializer<TestReferenceRecord, TestReferenceRecord.Serializer>()
        .RegisterValueSerializer<TestRecordArray, TestRecordArray.Serializer>()
        .RegisterValueSerializer<TestSubRecord, TestSubRecord.Serializer>()
        .RegisterValueSerializer<ValueRecord, ValueRecord.Serializer>()
        .RegisterGenericReferenceSerializer(typeof(ReadOnlyArrayWrapper<>), typeof(ReadOnlyArrayWrapperSerializer<>));
    }
  }
}