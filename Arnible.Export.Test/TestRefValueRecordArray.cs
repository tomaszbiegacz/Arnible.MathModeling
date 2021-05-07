using System;

namespace Arnible.Export.Test
{
  public ref struct TestRefValueRecordArray
  {
    public char? Pos { get; set; }
    public float? Value { get; set; }
    
    public decimal Price { get; set; }
    
    public decimal? Gross { get; set; }
    
    public ReadOnlySpan<char> Description { get; set; }
    
    public ReadOnlySpan<int> Values { get; set; }
    
    public Span<TestSubValueRecord> References { get; set; }
    
    /// <summary>
    /// Serializes record
    /// </summary>
    public readonly void Serialize(IRecordFieldSerializer serializer)
    {
      serializer.Write(nameof(Pos), Pos);
      serializer.Write(nameof(Value), Value);
      serializer.Write(nameof(Price), Price);
      serializer.Write(nameof(Gross), Gross);
      serializer.Write(nameof(Description), Description);
      serializer.CollectionField<int>().Write(nameof(Values), Values);
      serializer.CollectionField<TestSubValueRecord>().Write(nameof(References), References);
    }
  }
}