using Arnible.MathModeling;

namespace Arnible.Export
{
  public static class RecordWriterBuilderExtensions
  {
    public static RecordWriterBuilder RegisterMathModellingSerializers(this RecordWriterBuilder src)
    {
      return src
        .RegisterValueSerializer<Number,Number.Serializer>();
    }
  }
}