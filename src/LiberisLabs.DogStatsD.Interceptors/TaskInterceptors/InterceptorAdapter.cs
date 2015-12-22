using System;
using System.Reflection;
using System.Threading.Tasks;
using LiberisLabs.DogStatsD.Interceptors.Interceptors;

namespace LiberisLabs.DogStatsD.Interceptors.TaskInterceptors
{
    public class InterceptorAdapter : ITaskInterceptor
    {
        private readonly IInterceptor _interceptor;

        public InterceptorAdapter(IInterceptor interceptor)
        {
            _interceptor = interceptor;
        }

        public void OnEntry()
        {
            _interceptor.OnEntry();
        }

        public void OnExit()
        {
            _interceptor.OnExit();
        }

        public void OnException(Exception exception)
        {
            _interceptor.OnException(exception);
        }

        public bool CanIntercept(MethodInfo methodInfo, MethodInfo methodInvocationTarget)
        {
            return _interceptor.CanIntercept(methodInfo, methodInvocationTarget);
        }

        public void OnTaskContinuation(Task task)
        {
            
        }
    }
}