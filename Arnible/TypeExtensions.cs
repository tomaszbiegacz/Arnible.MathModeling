using System;

namespace Arnible
{
  public static class TypeExtensions
  {
    public static bool HasParameterlessConstructor(this Type classType)
    {
      return classType.GetConstructor(Type.EmptyTypes) is not null;
    }
    
    public static bool IsImplementingGenericInterface(this Type classOpenType, Type interfaceOpenType)
    {
      if(!interfaceOpenType.IsGenericTypeDefinition || !interfaceOpenType.IsInterface)
      {
        throw new ArgumentException(nameof(interfaceOpenType));
      }
      
      foreach(Type interfaceType in classOpenType.GetInterfaces())
      {
        if(interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == interfaceOpenType)
        {
          return true;
        }
      }
      return false;
    }
  }
}