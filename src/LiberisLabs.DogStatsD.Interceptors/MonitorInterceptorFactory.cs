using System;
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
                var timerInterceptor = CreateTimerInterceptor(methodInvocationTarget);

                monitorInterceptor.Add(timerInterceptor);
            }

            return monitorInterceptor;
        }

        private IInterceptor CreateTimerInterceptor(MethodInfo methodInvocationTarget)
        {
            IInterceptor timerInterceptor;
            if (IsTaskReturnType(methodInvocationTarget.ReturnType))
            {
                timerInterceptor = new TaskTimerInterceptor(methodInvocationTarget, _dogStatsD);
            }
            else
            {
                timerInterceptor = new TimerInterceptor(methodInvocationTarget, _dogStatsD);
            }
            return timerInterceptor;
        }

        private IInterceptor CreateInstrumentInterceptor(MethodInfo methodInvocationTarget)
        {
            var monitor = new InstrumentMonitor(methodInvocationTarget, _dogStatsD);

            return new MonitorInterceptorAdapter(monitor, IsTaskReturnType(methodInvocationTarget.ReturnType));
        }

        private static bool IsTaskReturnType(Type returnType)
        {
            return typeof(Task).IsAssignableFrom(returnType);
        }
    }
}