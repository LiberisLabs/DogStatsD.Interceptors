using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using LiberisLabs.DogStatsD.Interceptors.TaskInterceptors;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.DogStatsdInterceptorTests
{
    [TestFixture]
    public class DogStatsdInterceptorTestsForException
    {
        private Mock<ITaskInterceptor> _interceptor1;
        private Mock<ITaskInterceptor> _interceptor2;
        private Exception _exception;

        [SetUp]
        public void GivenADogStatsdInterceptor_WhenInterceptASuccessfulCall()
        {
            _exception = new Exception();
            _interceptor1 = new Mock<ITaskInterceptor>();
            _interceptor2 = new Mock<ITaskInterceptor>();

            var factory = new Mock<IInterceptorFactory>();
            factory.Setup(x => x.CreateInterceptors(It.IsAny<MethodInfo>(), It.IsAny<MethodInfo>()))
                .Returns(new List<ITaskInterceptor> { _interceptor1.Object, _interceptor2.Object });

            var interceptor = new DogStatsdInterceptor(factory.Object);

            var invocation = new Mock<IInvocation>();
            invocation.Setup(x => x.Proceed())
                .Throws(_exception);

            try
            {
                interceptor.Intercept(invocation.Object);

            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Test]
        public void ThenAllInterceptorsOnEntryIsCalledOnce()
        {
            _interceptor1.Verify(x => x.OnEntry(), Times.Once);
            _interceptor2.Verify(x => x.OnEntry(), Times.Once);
        }

        [Test]
        public void ThenInterceptorsOnExitIsNeverCalledOnce()
        {
            _interceptor1.Verify(x => x.OnExit(), Times.Never);
            _interceptor2.Verify(x => x.OnExit(), Times.Never);
        }

        [Test]
        public void ThenAllInterceptorsOnExceptionIsCalled()
        {
            _interceptor1.Verify(x => x.OnException(_exception), Times.Once);
            _interceptor2.Verify(x => x.OnException(_exception), Times.Once);
        }

        [Test]
        public void ThenInterceptorsOnTaskContinuationIsNeverCalled()
        {
            _interceptor1.Verify(x => x.OnTaskContinuation(It.IsAny<Task>()), Times.Never);
            _interceptor2.Verify(x => x.OnTaskContinuation(It.IsAny<Task>()), Times.Never);
        }
    }
}