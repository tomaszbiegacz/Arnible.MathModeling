namespace Arnible
{
  public delegate TResult FuncIn<T, out TResult>(in T arg);
  
  public delegate TResult FuncIn<T1, T2, out TResult>(in T1 arg1, in T2 arg2);
}