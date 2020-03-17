using Arnible.MathModeling.Export;

namespace Arnible.MathModeling.Test.Export
{
  [RecordSerializer(SerializationMediaType.TabSeparatedValues)]
  public struct TestSubRecord
  {
    public TestSubRecord(int value)
    {
      Value = value;
      HiddenValue = value;
    }

    public int Value { get; }

    [RecordPropertyIgnore]
    public int HiddenValue { get; }
  }
}
