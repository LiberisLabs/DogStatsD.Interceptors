using System;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Castle.DynamicProxy;

namespace LiberisLabs.DogStatsD.Interceptors
{
    public class DogStatdInterceptor : IInterceptor
    {
        private readonly IInterceptorFactory _factory;

        public DogStatdInterceptor(IInterceptorFactory factory)
        {
            _factory = factory;
        }

        public DogStatdInterceptor()
            : this(new InterceptorFactory())
        {
        }

        public void Intercept(IInvocation invocation)
        {
            var interceptors = _factory.CreateInterceptors(invocation.Method, invocation.MethodInvocationTarget);
            interceptors.ForEach(x => x.OnEntry());

            try
            {
                invocation.Proceed();

                interceptors.ForEach(x => x.OnExit());
            }
            catch (Exception ex)
            {
                interceptors.ForEach(x => x.OnException(ex));

                throw;
            }
            finally
            {
                var task = invocation.ReturnValue as Task;
                if (task != null)
                {
                    interceptors.ForEach(x => task.ContinueWith(x.OnTaskContinuation));
                }
            }
        }
    }
}
