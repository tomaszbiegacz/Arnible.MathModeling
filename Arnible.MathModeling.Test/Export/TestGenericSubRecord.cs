namespace Arnible.MathModeling.Test.Export
{
  public readonly struct TestGenericSubRecord<TProperty>
  {
    public TestGenericSubRecord(TProperty value)
    {
      Property = value;
    }

    public TProperty Property { get; }
  }
}
