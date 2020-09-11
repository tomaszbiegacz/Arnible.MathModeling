using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Arnible.MathModeling.xunit.AssertNumber;

namespace Arnible.MathModeling.Export.Test
{
  public class TsvSerializerTest
  {
    [Fact]
    public async Task DefaultSerialization()
    {
      var record = new TestRecord
      {
        ByteValue = 1,
        SbyteValue = -1,
        UShortValue = 2,
        ShortValue = -2,
        UIntValue = 3,
        IntValue = -3,
        ULongValue = 4,
        LongValue = -4,
        StringValue = "value",
        DoubleValue = 1.1,
        NumberValue = 1.2,
        NumberArray = new Number[] { 1.3, 1.4 }
      };

      string result;
      await using (MemoryStream stream = new MemoryStream())
      {
        var streamWriter = new TsvStreamWriter(stream);
        var serializer = TestRecord.CreateSerializer(streamWriter.FieldSerializer);

        serializer.Write(in record);
        await streamWriter.Flush();

        stream.Position = 0;
        var bytes = stream.ToArray();
        result = Encoding.UTF8.GetString(bytes);
      }

      string[] lines = result.Split('\n');
      AreEqual(3, lines.Length);
      AreEquals(new[]
      {
        "ByteValue", "SbyteValue", "UShortValue", "ShortValue", "UIntValue", "IntValue", "ULongValue", "LongValue", 
        "StringValue", "DoubleValue", "NumberValue", "NumberArray_0", "NumberArray_1"
      }, lines[0].Split('\t'));
      AreEqual("1\t-1\t2\t-2\t3\t-3\t4\t-4\tvalue\t1.1\t1.2\t1.3\t1.4", lines[1]);
      IsEmpty(lines[2]);
    }

    [Fact]
    public async Task InlineSerialization()
    {
      var record = new TestComplexRecord
      {
        RootValue = 1,
        Record = new TestSubRecord(2),
        Nullable = null,
        OtherValue = 3
      };

      string result;
      await using (MemoryStream stream = new MemoryStream())
      {
        var streamWriter = new TsvStreamWriter(stream);
        var serializer = TestComplexRecord.CreateSerializer(streamWriter.FieldSerializer);

        serializer.Write(in record);
        await streamWriter.Flush();

        stream.Position = 0;
        var bytes = stream.ToArray();
        result = Encoding.UTF8.GetString(bytes);
      }

      string[] lines = result.Split('\n');
      AreEqual(3, lines.Length);
      AreEquals(new[] { "RootValue", "Record_Value", "Nullable_NotPresentValue", "Nullable_NotPresentOther", "OtherValue" }, lines[0].Split('\t'));
      AreEqual("1\t2\t\t\t3", lines[1]);
      IsEmpty(lines[2]);
    }

    [Fact]
    public async Task ArraySerialization()
    {
      var record = new TestRecordArray
      {
        Records = new[] {
          new NullableSubRecord(),
          new NullableSubRecord
          {
            NotPresentValue = 1,
            NotPresentOther = 5
          }
          
        }
      };

      string result;
      await using (MemoryStream stream = new MemoryStream())
      {
        var streamWriter = new TsvStreamWriter(stream);
        var serializer = TestRecordArray.CreateSerializer(streamWriter.FieldSerializer);

        serializer.Write(in record);
        await streamWriter.Flush();

        stream.Position = 0;
        var bytes = stream.ToArray();
        result = Encoding.UTF8.GetString(bytes);
      }
      
      string[] lines = result.Split('\n');
      AreEqual(3, lines.Length);
      AreEquals(new[] { "Records_0_NotPresentValue", "Records_0_NotPresentOther", "Records_1_NotPresentValue", "Records_1_NotPresentOther" }, lines[0].Split('\t'));
      AreEqual("2\t3\t1\t5", lines[1]);
      IsEmpty(lines[2]);
    }
    
    [Fact]
    public async Task ArraySerialization_Empty()
    {
      var record = new TestRecordArray
      {
        Records = null
      };

      string result;
      await using (MemoryStream stream = new MemoryStream())
      {
        var streamWriter = new TsvStreamWriter(stream);
        var serializer = TestRecordArray.CreateSerializer(streamWriter.FieldSerializer);

        serializer.Write(in record);
        await streamWriter.Flush();

        stream.Position = 0;
        var bytes = stream.ToArray();
        result = Encoding.UTF8.GetString(bytes);
      }

      string[] lines = result.Split('\n');
      AreEqual(3, lines.Length);
      IsEmpty(lines[0]);
      IsEmpty(lines[1]);
      IsEmpty(lines[2]);
    }
  }
}
