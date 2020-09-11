namespace Arnible.MathModeling.Export
{
  public interface IRecordWriter<T>
  {
    void Write(in T record);

    void WriteNull();
  }
}