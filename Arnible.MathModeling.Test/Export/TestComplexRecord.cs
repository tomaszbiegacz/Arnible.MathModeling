namespace Arnible.MathModeling.Export.Test
{
  public struct TestComplexRecord
  {
    public int RootValue { get; set; }

    public TestSubRecord Record { get; set; }

    public NullableSubRecord Nullable { get; set; }

    public int OtherValue { get; set; }
  }
}
