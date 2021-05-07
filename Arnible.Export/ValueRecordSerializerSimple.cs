namespace Arnible.Export
{
  public abstract class ValueRecordSerializerSimple<T> : IValueRecordSerializer<T> where T: struct 
  {
    public void Serialize(IRecordFieldSerializer serializer, in T? record)
    {
      if (record.HasValue)
      {
        Serialize(serializer, record.Value);
      }
      else
      {
        Serialize(serializer, default);
      }
    }
    
    public abstract void Serialize(IRecordFieldSerializer serializer, in T record);
  }
}