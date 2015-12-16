using System.Collections.Generic;
using System.Reflection;
using LiberisLabs.DogStatsD.Interceptors.Interceptors;
using LiberisLabs.DogStatsD.Interceptors.TaskInterceptors;

namespace LiberisLabs.DogStatsD.Interceptors
{
    public interface IInterceptorFactory
    {
        ICollection<ITaskInterceptor> CreateInterceptors(MethodInfo method, MethodInfo methodInvocationTarget);
    }
}