using System.Collections.Generic;
using LiberisLabs.DogStatsD.Interceptors.TaskInterceptors;

namespace LiberisLabs.DogStatsD.Interceptors
{
    public interface IInterceptorRepository
    {
        IList<ITaskInterceptor> GetAll(string statName);
    }
}