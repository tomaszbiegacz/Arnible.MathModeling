namespace Arnible.Export
{
  public interface IRecordWriter
  {
    IRecordSerializer OpenRecord();
  }
}