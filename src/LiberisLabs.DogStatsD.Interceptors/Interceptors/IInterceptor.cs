using System;
using System.Reflection;

namespace LiberisLabs.DogStatsD.Interceptors.Interceptors
{
    public interface IInterceptor
    {
        void OnEntry();

        void OnExit();

        void OnException(Exception exception);

        bool CanIntercept(MethodInfo methodInfo, MethodInfo methodInvocationTarget);
    }
}