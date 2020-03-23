using Arnible.MathModeling.Export;

namespace Arnible.MathModeling.Test.Export
{
  public struct TestSubRecord
  {
    public TestSubRecord(int value)
    {
      Value = value;
      HiddenValue = value;
    }

    public int Value { get; }

    [RecordSerializerIgnore]
    public int HiddenValue { get; }
  }
}
