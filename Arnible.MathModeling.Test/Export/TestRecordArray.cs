using Arnible.MathModeling.Export;

namespace Arnible.MathModeling.Test.Export
{
  public struct TestRecordArray
  {
    [FixedArraySerializer(3)]
    public NullableSubRecord[] Records { get; set; }
  }
}
