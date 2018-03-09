using System;
using System.Linq;
using System.Reflection;

namespace LiberisLabs.DogStatsD.Interceptors.Interceptors
{
    public static class MethodInfoExtensions
    {
        public static bool HasAttribute<T>(this MethodInfo methodInfo) where T : Attribute
        {
            var attrs = methodInfo.GetCustomAttributes(typeof(T), false);

            return attrs != null && attrs.Any();
        }
    }
}