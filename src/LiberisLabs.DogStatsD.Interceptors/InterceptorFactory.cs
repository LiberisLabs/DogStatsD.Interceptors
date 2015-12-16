using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LiberisLabs.DogStatsD.Interceptors.TaskInterceptors;

namespace LiberisLabs.DogStatsD.Interceptors
{
    public class InterceptorFactory : IInterceptorFactory
    {
        private readonly IInterceptorRepository _interceptorRepository;
        private readonly IStatNameCreator _statNameCreator;

        public InterceptorFactory(IInterceptorRepository interceptorRepository, IStatNameCreator statNameCreator)
        {
            _interceptorRepository = interceptorRepository;
            _statNameCreator = statNameCreator;
        }

        public InterceptorFactory()
            :this(new InterceptorRepository(), new StatNameCreator())
        {
            
        }
        
        public ICollection<ITaskInterceptor> CreateInterceptors(MethodInfo method, MethodInfo methodInvocationTarget)
        {
            var statName = _statNameCreator.Create(methodInvocationTarget);

            var applicableInterceptors = _interceptorRepository.GetAll(statName)
                .Where(x => x.CanIntercept(method, methodInvocationTarget))
                .ToList();

            return applicableInterceptors;
        }
    }
}