using System.Reflection;
using System.Threading.Tasks;
using LiberisLabs.DogStatsD.Interceptors.Annotations;
using LiberisLabs.DogStatsD.Interceptors.TaskInterceptors;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.TaskInterceptors.TaskTimerInterceptorTests
{
    [TestFixture]
    public class TaskTimerInterceptorCanInterceptForTimeAttribute
    {
        private TaskTimerInterceptor _interceptor;

        [SetUp]
        public void GivenATaskTimerInterceptor()
        {
            _interceptor = new TaskTimerInterceptor(null, null);
        }

        [Test]
        public void WhenCheckingIfCanInterceptForTimeAttribute_ThenTheResultIsTrue()
        {
            var methodInfo = new Mock<MethodInfo>();
            methodInfo.Setup(x => x.GetCustomAttributes(typeof (TimeAttribute), false))
                .Returns(new [] {new TimeAttribute()});
            methodInfo.Setup(x => x.ReturnType)
                .Returns(typeof (Task));

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
                .Returns(typeof(Task));

            var result = _interceptor.CanIntercept(methodInfo.Object, methodInvocationTarget.Object);

            Assert.IsTrue(result);
        }
    }
}
