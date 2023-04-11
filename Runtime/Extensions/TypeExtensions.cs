using System;

namespace DI.Extensions
{
    public static class TypeExtensions
    {
        public static bool Implements(this Type type, Type interfaceType)
        {
            return interfaceType.IsAssignableFrom(type);
        }
    }
}