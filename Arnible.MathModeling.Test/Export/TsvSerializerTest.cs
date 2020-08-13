using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Test.Export;
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
        NumberVectorValue = new NumberVector(1, 2)
      };

      string result;
      var serializer = new TsvSerializer<TestRecord>();
      await using (MemoryStream stream = new MemoryStream())
      {
        await serializer.Serialize(new[] { record }, stream, default);

        stream.Position = 0;
        var bytes = stream.ToArray();
        result = Encoding.UTF8.GetString(bytes);
      }

      string[] lines = result.Split('\n');
      AreEqual(3, lines.Length);
      AreEquals(new[] { "ByteValue", "SbyteValue", "UShortValue", "ShortValue", "UIntValue", "IntValue", "ULongValue", "LongValue", "StringValue", "DoubleValue", "NumberValue", "NumberVectorValue" }, lines[0].Split('\t'));
      AreEqual("1\t-1\t2\t-2\t3\t-3\t4\t-4\tvalue\t1.1\t1.2\t[1 2]", lines[1]);
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
      var serializer = new TsvSerializer<TestComplexRecord>();
      await using (MemoryStream stream = new MemoryStream())
      {
        await serializer.Serialize(new[] { record }, stream, default);

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
          null,
          new NullableSubRecord(),
          null
        }
      };

      string result;
      var serializer = new TsvSerializer<TestRecordArray>();
      await using (MemoryStream stream = new MemoryStream())
      {
        await serializer.Serialize(new[] { record }, stream, default);

        stream.Position = 0;
        var bytes = stream.ToArray();
        result = Encoding.UTF8.GetString(bytes);
      }
      
      string[] lines = result.Split('\n');
      AreEqual(3, lines.Length);
      AreEquals(new[] { "Records_0_NotPresentValue", "Records_0_NotPresentOther", "Records_1_NotPresentValue", "Records_1_NotPresentOther", "Records_2_NotPresentValue", "Records_2_NotPresentOther" }, lines[0].Split('\t'));
      AreEqual("\t\t2\t3\t\t", lines[1]);
      IsEmpty(lines[2]);
    }

    [Fact]
    public async Task ArraySerialization_Underscaled()
    {
      var record = new TestRecordArray
      {
        Records = new[] {
          null,
          new NullableSubRecord()
        }
      };

      string result;
      var serializer = new TsvSerializer<TestRecordArray>();
      await using (MemoryStream stream = new MemoryStream())
      {
        await serializer.Serialize(new[] { record }, stream, default);

        stream.Position = 0;
        var bytes = stream.ToArray();
        result = Encoding.UTF8.GetString(bytes);
      }

      string[] lines = result.Split('\n');
      AreEqual(3, lines.Length);
      AreEquals(new[] { "Records_0_NotPresentValue", "Records_0_NotPresentOther", "Records_1_NotPresentValue", "Records_1_NotPresentOther", "Records_2_NotPresentValue", "Records_2_NotPresentOther" }, lines[0].Split('\t'));
      AreEqual("\t\t2\t3\t\t", lines[1]);
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
      var serializer = new TsvSerializer<TestRecordArray>();
      await using (MemoryStream stream = new MemoryStream())
      {
        await serializer.Serialize(new[] { record }, stream, default);

        stream.Position = 0;
        var bytes = stream.ToArray();
        result = Encoding.UTF8.GetString(bytes);
      }

      string[] lines = result.Split('\n');
      AreEqual(3, lines.Length);
      AreEquals(new[] { "Records_0_NotPresentValue", "Records_0_NotPresentOther", "Records_1_NotPresentValue", "Records_1_NotPresentOther", "Records_2_NotPresentValue", "Records_2_NotPresentOther" }, lines[0].Split('\t'));
      AreEqual("\t\t\t\t\t", lines[1]);
      IsEmpty(lines[2]);
    }

    [Fact]
    public async Task GenericSerializer()
    {
      var serializerNumber = new TsvSerializer<TestGenericRecord<Number>>();
      var serializerVector = new TsvSerializer<TestGenericRecord<NumberVector>>();

      var recordNumber = new TestGenericRecord<Number>
      {
        Output = 1,
        Error = 2,
        ErrorVector = new NumberVector(1, 2),
        SubOutput = new TestGenericSubRecord<Number>(5)
      };
      await using (MemoryStream stream = new MemoryStream())
      {
        await serializerNumber.Serialize(new[] { recordNumber }, stream, default);

        stream.Position = 0;
        var bytes = stream.ToArray();
        string result = Encoding.UTF8.GetString(bytes);

        string[] lines = result.Split('\n');
        AreEqual(3, lines.Length);
        AreEquals(new[] { "Output", "Error", "ErrorVector", "SubOutput_Property" }, lines[0].Split('\t'));
        AreEqual("1\t2\t[1 2]\t5", lines[1]);
        IsEmpty(lines[2]);
      }

      var recordVector = new TestGenericRecord<NumberVector>
      {
        Output = new NumberVector(1, 2),
        Error = 2,
        ErrorVector = new NumberVector(1, 2),
        SubOutput = new TestGenericSubRecord<NumberVector>(new NumberVector(3, 5))
      };
      await using (MemoryStream stream = new MemoryStream())
      {
        await serializerVector.Serialize(new[] { recordVector }, stream, default);

        stream.Position = 0;
        var bytes = stream.ToArray();
        string result = Encoding.UTF8.GetString(bytes);

        string[] lines = result.Split('\n');
        AreEqual(3, lines.Length);
        AreEquals(new[] { "Output", "Error", "ErrorVector", "SubOutput_Property" }, lines[0].Split('\t'));
        AreEqual("[1 2]\t2\t[1 2]\t[3 5]", lines[1]);
        IsEmpty(lines[2]);
      }
    }
  }
}
