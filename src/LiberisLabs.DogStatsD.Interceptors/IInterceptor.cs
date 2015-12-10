using System;
using System.Threading.Tasks;

namespace LiberisLabs.DogStatsD.Interceptors
{
    public interface IInterceptor
    {
        void OnEntry();

        void OnExit();

        void OnException(Exception exception);

        void OnTaskContinuation(Task task);
    }
}