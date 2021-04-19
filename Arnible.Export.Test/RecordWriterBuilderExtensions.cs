using Arnible.MathModeling;

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
        .RegisterValueSerializer<TestRecord, TestRecord.Serializer>()
        .RegisterValueSerializer<TestRecordArray, TestRecordArray.Serializer>()
        .RegisterValueSerializer<TestSubRecord, TestSubRecord.Serializer>()
        .RegisterValueSerializer<ValueRecord, ValueRecord.Serializer>();
    }
  }
}