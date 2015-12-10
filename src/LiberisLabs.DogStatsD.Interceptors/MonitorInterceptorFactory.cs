using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Castle.Core.Internal;
using LiberisLabs.DogStatsD.Interceptors.Monitors;

namespace LiberisLabs.DogStatsD.Interceptors
{
    public class MonitorInterceptorFactory : IMonitorInterceptorFactory
    {
        private readonly IDogStatsd _dogStatsd;

        public MonitorInterceptorFactory()
        {
            _dogStatsd = new DogStatsdWrapper();
        }

        public ICollection<IInterceptor> CreateMonitorInterceptors(MethodInfo method, MethodInfo methodInvocationTarget)
        {
            var monitorInterceptor = new List<IInterceptor>();

            if (method.HasAttribute<InstrumentAttribute>() || methodInvocationTarget.HasAttribute<InstrumentAttribute>())
            {
                monitorInterceptor.Add(CreateInstrumentInterceptor(methodInvocationTarget));
            }

            return monitorInterceptor;
        }

        private IInterceptor CreateInstrumentInterceptor(MethodInfo methodInvocationTarget)
        {
            var monitor = new InstrumentMonitor(methodInvocationTarget, _dogStatsd);

            return new MonitorInterceptorAdapter(monitor, typeof(Task).IsAssignableFrom(methodInvocationTarget.ReturnType));
        }
    }
}