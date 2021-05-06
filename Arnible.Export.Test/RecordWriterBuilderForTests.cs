namespace Arnible.Export.Test
{
  public static class RecordWriterBuilderForTests
  {
    private static readonly RecordWriterBuilder _builder;
    
    static RecordWriterBuilderForTests()
    {
      _builder = new RecordWriterBuilder()
        .RegisterTestSerializers();
    }
    
    public static IRecordWriterBuilder Default => _builder;
  }
}