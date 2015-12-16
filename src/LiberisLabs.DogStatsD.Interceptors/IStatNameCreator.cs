using System.Reflection;

namespace LiberisLabs.DogStatsD.Interceptors
{
    public interface IStatNameCreator
    {
        string Create(MethodInfo methodInvocationTarget);
    }
}