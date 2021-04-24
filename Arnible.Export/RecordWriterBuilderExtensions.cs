namespace Arnible.Export
{
  public static class RecordWriterBuilderExtensions
  {
    public static RecordWriterBuilder RegisterCoreSerializers(this RecordWriterBuilder src)
    {
      return src
        .RegisterGenericValueSerializer(typeof(ReadOnlyArray<>), typeof(ReadOnlyArraySerializer<>));
    }
  }
}