using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.DogStatdInterceptorTests
{
    [TestFixture]
    public class DogStatdInterceptorTests
    {
        private Mock<IInterceptor> _interceptor1;
        private Mock<IInterceptor> _interceptor2;

        [SetUp]
        public void GivenADogStatdInterceptor_WhenInterceptASuccessfulCall()
        {
            _interceptor1 = new Mock<IInterceptor>();
            _interceptor2 = new Mock<IInterceptor>();

            var factory = new Mock<IMonitorInterceptorFactory>();
            factory.Setup(x => x.CreateMonitorInterceptors(It.IsAny<MethodInfo>(), It.IsAny<MethodInfo>()))
                .Returns(new List<IInterceptor>() {_interceptor1.Object, _interceptor2.Object});

            var interceptor = new DogStatdInterceptor(factory.Object);

            var invocation = new Mock<IInvocation>();
            invocation.Setup(x => x.ReturnValue).Returns(1);

            interceptor.Intercept(invocation.Object);
        }

        [Test]
        public void ThenAllInterceptorsOnEntryIsCalledOnce()
        {
            _interceptor1.Verify(x => x.OnEntry(), Times.Once);
            _interceptor2.Verify(x => x.OnEntry(), Times.Once);
        }

        [Test]
        public void ThenAllInterceptorsOnExitIsCalledOnce()
        {
            _interceptor1.Verify(x => x.OnExit(), Times.Once);
            _interceptor2.Verify(x => x.OnExit(), Times.Once);
        }

        [Test]
        public void ThenInterceptorsOnExceptionIsNeverCalled()
        {
            _interceptor1.Verify(x => x.OnException(It.IsAny<Exception>()), Times.Never);
            _interceptor2.Verify(x => x.OnException(It.IsAny<Exception>()), Times.Never);
        }

        [Test]
        public void ThenInterceptorsOnTaskContinuationIsNeverCalled()
        {
            _interceptor1.Verify(x => x.OnTaskContinuation(It.IsAny<Task>()), Times.Never);
            _interceptor2.Verify(x => x.OnTaskContinuation(It.IsAny<Task>()), Times.Never);
        }
    }
}
