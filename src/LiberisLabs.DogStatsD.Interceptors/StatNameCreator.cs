using System.Reflection;

namespace LiberisLabs.DogStatsD.Interceptors
{
    public class StatNameCreator : IStatNameCreator
    {
        public string Create(MethodInfo methodInvocationTarget)
        {
            return $"{methodInvocationTarget.ReflectedType.FullName}.{methodInvocationTarget.Name}".ToLowerInvariant();
        }
    }
}