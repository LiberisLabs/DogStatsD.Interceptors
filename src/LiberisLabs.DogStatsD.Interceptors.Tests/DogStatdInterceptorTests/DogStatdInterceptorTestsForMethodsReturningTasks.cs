using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using LiberisLabs.DogStatsD.Interceptors.TaskInterceptors;
using Moq;
using NUnit.Framework;
using IInterceptor = LiberisLabs.DogStatsD.Interceptors.Interceptors.IInterceptor;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.DogStatdInterceptorTests
{
    [TestFixture]
    public class DogStatdInterceptorTestsForMethodsReturningTasks
    {
        private Mock<ITaskInterceptor> _interceptor1;
        private Mock<ITaskInterceptor> _interceptor2;
        private Task _task;

        [SetUp]
        public void GivenADogStatdInterceptor_WhenInterceptASuccessfulCall()
        {
            _task = Task.FromResult(0);
            _interceptor1 = new Mock<ITaskInterceptor>();
            _interceptor2 = new Mock<ITaskInterceptor>();

            var factory = new Mock<IInterceptorFactory>();
            factory.Setup(x => x.CreateInterceptors(It.IsAny<MethodInfo>(), It.IsAny<MethodInfo>()))
                .Returns(new List<ITaskInterceptor>() { _interceptor1.Object, _interceptor2.Object });

            var interceptor = new DogStatdInterceptor(factory.Object);

            var invocation = new Mock<IInvocation>();
            invocation.Setup(x => x.ReturnValue)
                .Returns(_task);

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
        public void ThenAllInterceptorsOnTaskContinuationIsCalled()
        {
            _interceptor1.Verify(x => x.OnTaskContinuation(_task), Times.Once);
            _interceptor2.Verify(x => x.OnTaskContinuation(_task), Times.Once);
        }
    }
}