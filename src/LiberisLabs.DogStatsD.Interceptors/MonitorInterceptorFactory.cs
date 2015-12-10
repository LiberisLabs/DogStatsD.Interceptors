using System.Collections.Generic;
using System.Reflection;
using Castle.Core.Internal;

namespace LiberisLabs.DogStatsD.Interceptors
{
    public class MonitorInterceptorFactory
    {
        private readonly IDogStatsd _dogStatsd;

        public MonitorInterceptorFactory()
        {
            _dogStatsd = new DogStatsdWrapper();
        }

        public ICollection<IMonitor> CreateMonitorInterceptors(MethodInfo method, MethodInfo methodInvocationTarget)
        {
            var monitorInterceptor = new List<IMonitor>();

            if (method.HasAttribute<InstrumentAttribute>() || methodInvocationTarget.HasAttribute<InstrumentAttribute>())
            {
                monitorInterceptor.Add(new DataDogInstrumentMonitor(methodInvocationTarget, _dogStatsd));
            }

            return monitorInterceptor;
        }

    }
}