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
      using(var stream = new MemoryStream())
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
  }
}
