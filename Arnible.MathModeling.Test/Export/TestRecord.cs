namespace Arnible.MathModeling.Export.Test
{
  public struct TestRecord
  {
    public byte ByteValue { get; set; }

    public sbyte SbyteValue { get; set; }

    public ushort UShortValue { get; set; }

    public short ShortValue { get; set; }

    public uint UIntValue { get; set; }

    public int IntValue { get; set; }

    public ulong ULongValue { get; set; }

    public long LongValue { get; set; }

    public string StringValue { get; set; }

    public double DoubleValue { get; set; }

    public Number NumberValue { get; set; }
    
    public ValueArray<Number> NumberArray { get; set; }
    
    //
    // Serialization
    //
    
    class Serializer : IRecordWriter<TestRecord>
    {
      private readonly IRecordFieldSerializer _serializer;
      private readonly IRecordWriter<Number> _serializerNumberValue;
      private readonly IRecordWriter<ValueArray<Number>> _serializerNumberArray;
      
      public Serializer(in IRecordFieldSerializer serializer)
      {
        _serializer = serializer;
        _serializerNumberValue = serializer.GetRecordSerializer(nameof(NumberValue), Number.CreateSerializer);
        _serializerNumberArray = serializer.GetRecordSerializer(nameof(NumberArray),
          s => ValueArray<Number>.CreateSerializer(s, Number.CreateSerializer));
      }
      
      public void Write(in TestRecord record)
      {
        _serializer.Write(nameof(ByteValue), record.ByteValue);
        _serializer.Write(nameof(SbyteValue), record.SbyteValue);
        _serializer.Write(nameof(UShortValue), record.UShortValue);
        _serializer.Write(nameof(ShortValue), record.ShortValue);
        _serializer.Write(nameof(UIntValue), record.UIntValue);
        _serializer.Write(nameof(IntValue), record.IntValue);
        _serializer.Write(nameof(ULongValue), record.ULongValue);
        _serializer.Write(nameof(LongValue), record.LongValue);
        _serializer.Write(nameof(StringValue), record.StringValue);
        _serializer.Write(nameof(DoubleValue), record.DoubleValue);
        _serializerNumberValue.Write(record.NumberValue);
        _serializerNumberArray.Write(record.NumberArray);
        _serializer.CommitWrite();
      }

      public void WriteNull()
      {
        _serializer.WriteNull(nameof(ByteValue));
        _serializer.WriteNull(nameof(SbyteValue));
        _serializer.WriteNull(nameof(UShortValue));
        _serializer.WriteNull(nameof(ShortValue));
        _serializer.WriteNull(nameof(UIntValue));
        _serializer.WriteNull(nameof(IntValue));
        _serializer.WriteNull(nameof(ULongValue));
        _serializer.WriteNull(nameof(LongValue));
        _serializer.WriteNull(nameof(StringValue));
        _serializer.WriteNull(nameof(DoubleValue));
        _serializerNumberValue.WriteNull();
        _serializerNumberArray.WriteNull();
        _serializer.CommitWrite();
      }
    }
    
    public static IRecordWriter<TestRecord> CreateSerializer(IRecordFieldSerializer serializer)
    {
      return new Serializer(in serializer);
    }
  }
}
