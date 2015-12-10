using System.Collections.Generic;
using System.Reflection;
using Castle.Core.Internal;

namespace LiberisLabs.DogStatsD.Interceptors
{
    public class MonitorInterceptorFactory
    {
        public ICollection<IMonitor> CreateMonitorInterceptors(MethodInfo method, MethodInfo methodInvocationTarget)
        {
            var monitorInterceptor = new List<IMonitor>();

            if (method.HasAttribute<InstrumentAttribute>() || methodInvocationTarget.HasAttribute<InstrumentAttribute>())
            {
                monitorInterceptor.Add(new DataDogInstrumentMonitor(methodInvocationTarget));
            }

            return monitorInterceptor;
        }

    }
}