using Arnible.Export;

namespace Arnible.MathModeling
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