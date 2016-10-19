using System.Reflection;
using LiberisLabs.DogStatsD.Interceptors.TaskInterceptors;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.TaskInterceptors.TaskTimerInterceptorTests
{
    [TestFixture]
    public class TaskTimerInterceptorCanInterceptForNoneTaskMethod
    {
        private TaskTimerInterceptor _interceptor;

        [SetUp]
        public void GivenATaskTimerInterceptor()
        {
            _interceptor = new TaskTimerInterceptor(null, null);
        }

        [Test]
        public void WhenCheckingIfCanInterceptForNoneTaskMethod_ThenTheResultIsFalse()
        {
            var methodInfo = new Mock<MethodInfo>();
            methodInfo.Setup(x => x.ReturnType)
                .Returns(typeof(int));

            var result = _interceptor.CanIntercept(methodInfo.Object, Mock.Of<MethodInfo>());

            Assert.IsFalse(result);
        }
    }
}