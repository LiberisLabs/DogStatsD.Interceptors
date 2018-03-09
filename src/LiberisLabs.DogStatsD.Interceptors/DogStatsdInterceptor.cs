using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using System.Linq;

namespace LiberisLabs.DogStatsD.Interceptors
{
    public class DogStatsdInterceptor : IInterceptor
    {
        private readonly IInterceptorFactory _factory;

        public DogStatsdInterceptor(IInterceptorFactory factory)
        {
            _factory = factory;
        }

        public DogStatsdInterceptor()
            : this(new InterceptorFactory())
        {
        }

        public void Intercept(IInvocation invocation)
        {
            var interceptors = _factory.CreateInterceptors(invocation.Method, invocation.MethodInvocationTarget).ToList();
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
                if (invocation.ReturnValue is Task task)
                {
                    interceptors.ForEach(x => task.ContinueWith(x.OnTaskContinuation));
                }
            }
        }
    }
}
