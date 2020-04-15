namespace Arnible.MathModeling.Export.Test
{
  public struct TestRecordArray
  {
    [FixedArraySerializer(3)]
    public NullableSubRecord[] Records { get; set; }
  }
}
