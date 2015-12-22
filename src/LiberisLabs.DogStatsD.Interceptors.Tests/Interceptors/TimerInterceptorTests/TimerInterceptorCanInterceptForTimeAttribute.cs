using System.Reflection;
using LiberisLabs.DogStatsD.Interceptors.Annotations;
using LiberisLabs.DogStatsD.Interceptors.Interceptors;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.Interceptors.TimerInterceptorTests
{
    [TestFixture]
    public class TimerInterceptorCanInterceptForTimeAttribute
    {
        private TimerInterceptor _interceptor;

        [SetUp]
        public void GivenATimerInterceptor()
        {
            _interceptor = new TimerInterceptor(null, null);
        }

        [Test]
        public void WhenCheckingIfCanInterceptForTimeAttribute_ThenTheResultIsTrue()
        {
            var methodInfo = new Mock<MethodInfo>();
            methodInfo.Setup(x => x.GetCustomAttributes(typeof (TimeAttribute), false))
                .Returns(new [] {new TimeAttribute()});
            methodInfo.Setup(x => x.ReturnType)
                .Returns(typeof (void));

            var result = _interceptor.CanIntercept(methodInfo.Object, new Mock<MethodInfo>().Object);

            Assert.IsTrue(result);
        }

        [Test]
        public void WhenCheckingIfCanInterceptForTimeAttributeOnTarget_ThenTheResultIsTrue()
        {
            var methodInvocationTarget = new Mock<MethodInfo>();
            methodInvocationTarget.Setup(x => x.GetCustomAttributes(typeof(TimeAttribute), false))
                .Returns(new[] { new TimeAttribute() });

            var methodInfo = new Mock<MethodInfo>();
            methodInfo.Setup(x => x.GetCustomAttributes(typeof(TimeAttribute), false))
                .Returns(new TimeAttribute[0]);
            methodInfo.Setup(x => x.ReturnType)
                .Returns(typeof(void));

            var result = _interceptor.CanIntercept(methodInfo.Object, methodInvocationTarget.Object);

            Assert.IsTrue(result);
        }
    }
}
