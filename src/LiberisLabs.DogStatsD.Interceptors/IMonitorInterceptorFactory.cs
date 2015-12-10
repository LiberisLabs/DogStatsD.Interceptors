using System.Collections.Generic;
using System.Reflection;

namespace LiberisLabs.DogStatsD.Interceptors
{
    public interface IMonitorInterceptorFactory
    {
        ICollection<IInterceptor> CreateMonitorInterceptors(MethodInfo method, MethodInfo methodInvocationTarget);
    }
}