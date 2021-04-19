namespace Arnible.Export.Test
{
  public static class TestRecordWriterBuilder
  {
    private static readonly RecordWriterBuilder _builder;
    
    static TestRecordWriterBuilder()
    {
      _builder = new RecordWriterBuilder()
        .RegisterTestSerializers();
    }
    
    public static IRecordWriterBuilder Default => _builder;
  }
}