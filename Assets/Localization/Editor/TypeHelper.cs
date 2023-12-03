using System;
using System.Linq;

namespace MbsCore.Localization.Editor
{
    internal static class TypeHelper
    {
        public static Type[] GetImplementations(this Type baseType)
        {
            bool ImplementationCondition(Type type) =>
                    baseType.IsAssignableFrom(type) && type != baseType
                    && !type.IsAbstract && !type.IsGenericType;

            return baseType.GetAllMatchingTypes(ImplementationCondition);
        }
        
        private static Type[] GetAllMatchingTypes(this Type baseType, Func<Type, bool> predicate)
        {
            return AppDomain.CurrentDomain
                            .GetAssemblies()
                            .SelectMany(s => s.GetTypes())
                            .Where(predicate)
                            .ToArray();
        }
    }
}