using Arnible.MathModeling.Algebra;
using Arnible.MathModeling.Export;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Arnible.MathModeling.Test.Export
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

      string[] lines = result.Split(Environment.NewLine);
      Assert.Equal(3, lines.Length);
      Assert.Equal(new[] { "ByteValue", "SbyteValue", "UShortValue", "ShortValue", "UIntValue", "IntValue", "ULongValue", "LongValue", "StringValue", "DoubleValue", "NumberValue", "NumberVectorValue" }, lines[0].Split('\t'));
      Assert.Equal("1\t-1\t2\t-2\t3\t-3\t4\t-4\tvalue\t1.1\t1.2\t[1 2]", lines[1]);
      Assert.Empty(lines[2]);
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

      string[] lines = result.Split(Environment.NewLine);
      Assert.Equal(3, lines.Length);
      Assert.Equal(new[] { "RootValue", "Record_Value", "Nullable_NotPresentValue", "Nullable_NotPresentOther", "OtherValue" }, lines[0].Split('\t'));
      Assert.Equal("1\t2\t\t\t3", lines[1]);
      Assert.Empty(lines[2]);
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

      string[] lines = result.Split(Environment.NewLine);
      Assert.Equal(3, lines.Length);
      Assert.Equal(new[] { "Records_0_NotPresentValue", "Records_0_NotPresentOther", "Records_1_NotPresentValue", "Records_1_NotPresentOther", "Records_2_NotPresentValue", "Records_2_NotPresentOther" }, lines[0].Split('\t'));
      Assert.Equal("\t\t2\t3\t\t", lines[1]);
      Assert.Empty(lines[2]);
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

      string[] lines = result.Split(Environment.NewLine);
      Assert.Equal(3, lines.Length);
      Assert.Equal(new[] { "Records_0_NotPresentValue", "Records_0_NotPresentOther", "Records_1_NotPresentValue", "Records_1_NotPresentOther", "Records_2_NotPresentValue", "Records_2_NotPresentOther" }, lines[0].Split('\t'));
      Assert.Equal("\t\t2\t3\t\t", lines[1]);
      Assert.Empty(lines[2]);
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

      string[] lines = result.Split(Environment.NewLine);
      Assert.Equal(3, lines.Length);
      Assert.Equal(new[] { "Records_0_NotPresentValue", "Records_0_NotPresentOther", "Records_1_NotPresentValue", "Records_1_NotPresentOther", "Records_2_NotPresentValue", "Records_2_NotPresentOther" }, lines[0].Split('\t'));
      Assert.Equal("\t\t\t\t\t", lines[1]);
      Assert.Empty(lines[2]);
    }
  }
}
