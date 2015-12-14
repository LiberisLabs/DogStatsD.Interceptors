using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Castle.Core.Internal;
using LiberisLabs.DogStatsD.Interceptors.Annotations;
using LiberisLabs.DogStatsD.Interceptors.Interceptors;
using LiberisLabs.DogStatsD.Interceptors.Monitors;

namespace LiberisLabs.DogStatsD.Interceptors
{
    public class MonitorInterceptorFactory : IMonitorInterceptorFactory
    {
        private readonly IDogStatsD _dogStatsD;

        public MonitorInterceptorFactory()
        {
            _dogStatsD = new DogStatsDWrapper();
        }

        public ICollection<IInterceptor> CreateMonitorInterceptors(MethodInfo method, MethodInfo methodInvocationTarget)
        {
            var monitorInterceptor = new List<IInterceptor>();

            if (method.HasAttribute<InstrumentAttribute>() || methodInvocationTarget.HasAttribute<InstrumentAttribute>())
            {
                monitorInterceptor.Add(CreateInstrumentInterceptor(methodInvocationTarget));
            }

            if (method.HasAttribute<TimeAttribute>() || methodInvocationTarget.HasAttribute<TimeAttribute>())
            {
                var timerInterceptor = new TimerInterceptor(methodInvocationTarget, _dogStatsD);
                monitorInterceptor.Add(timerInterceptor);
            }

            return monitorInterceptor;
        }

        private IInterceptor CreateInstrumentInterceptor(MethodInfo methodInvocationTarget)
        {
            var monitor = new InstrumentMonitor(methodInvocationTarget, _dogStatsD);

            return new MonitorInterceptorAdapter(monitor, typeof(Task).IsAssignableFrom(methodInvocationTarget.ReturnType));
        }
    }
}