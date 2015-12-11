using Autofac;
using Autofac.Extras.DynamicProxy2;
using LiberisLabs.DogStatsD.Interceptors;

namespace LiberisLabs.DogStatsD.FunctionalTests
{
    public class TestServiceFactory
    {
        public ITestService Create()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DogStatdInterceptor>();

            builder.RegisterType<TestService>()
                .InterceptedBy(typeof(DogStatdInterceptor))
                .As<ITestService>()
                .EnableInterfaceInterceptors();

            var container = builder.Build();

            var service = container.Resolve<ITestService>();

            return service;
        }
    }
}
