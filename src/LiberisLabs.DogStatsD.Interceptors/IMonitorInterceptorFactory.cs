using System.Collections.Generic;
using System.Reflection;
using LiberisLabs.DogStatsD.Interceptors.Interceptors;

namespace LiberisLabs.DogStatsD.Interceptors
{
    public interface IMonitorInterceptorFactory
    {
        ICollection<IInterceptor> CreateMonitorInterceptors(MethodInfo method, MethodInfo methodInvocationTarget);
    }
}