using System.Reflection;
using System.Threading.Tasks;
using LiberisLabs.DogStatsD.Interceptors.Interceptors;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.Interceptors.TimerInterceptorTests
{
    [TestFixture]
    public class TimerInterceptorCanInterceptForTaskMethod
    {
        private TimerInterceptor _interceptor;

        [SetUp]
        public void GivenATimerInterceptor()
        {
            _interceptor = new TimerInterceptor(null, null);
        }

        [Test]
        public void WhenCheckingIfCanInterceptForTaskMethod_ThenTheResultIsFalse()
        {
            var methodInfo = new Mock<MethodInfo>();
            methodInfo.Setup(x => x.ReturnType)
                .Returns(typeof(Task));

            var result = _interceptor.CanIntercept(methodInfo.Object, Mock.Of<MethodInfo>());

            Assert.IsFalse(result);
        }
    }
}