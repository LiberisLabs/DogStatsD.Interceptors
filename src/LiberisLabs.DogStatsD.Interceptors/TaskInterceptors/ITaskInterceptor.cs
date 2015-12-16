using System.Threading.Tasks;
using LiberisLabs.DogStatsD.Interceptors.Interceptors;

namespace LiberisLabs.DogStatsD.Interceptors.TaskInterceptors
{
    public interface ITaskInterceptor : IInterceptor
    {
        void OnTaskContinuation(Task task);
    }
}