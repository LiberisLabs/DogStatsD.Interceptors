using Autofac;
using Autofac.Extras.DynamicProxy;
using LiberisLabs.DogStatsD.Interceptors;

namespace LiberisLabs.DogStatsD.FunctionalTests
{
    public class TestServiceFactory
    {
        public ITestService Create()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<DogStatsdInterceptor>();

            builder.RegisterType<TestService>()
                .InterceptedBy(typeof(DogStatsdInterceptor))
                .As<ITestService>()
                .EnableInterfaceInterceptors();

            var container = builder.Build();

            var service = container.Resolve<ITestService>();

            return service;
        }
    }
}
