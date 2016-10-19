using System;
using LiberisLabs.DogStatsD.Interceptors.Interceptors;
using LiberisLabs.DogStatsD.Interceptors.TaskInterceptors;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.TaskInterceptors
{
    [TestFixture]
    public class InterceptorAdapterTests
    {
        private Mock<IInterceptor> _internalInterceptor;
        private InterceptorAdapter _adapter;

        [SetUp]
        public void GivenInterceptorAdapter()
        {
            _internalInterceptor = new Mock<IInterceptor>();
            _adapter = new InterceptorAdapter(_internalInterceptor.Object);
        }

        [Test]
        public void WhenOnEntryIsCalled_ThenTheInteralInterceptorIsCalled()
        {
            _adapter.OnEntry();

            _internalInterceptor.Verify(x => x.OnEntry(), Times.Once);
        }
        
        [Test]
        public void WhenOnExitIsCalled_ThenTheInteralInterceptorIsCalled()
        {
            _adapter.OnExit();

            _internalInterceptor.Verify(x => x.OnExit(), Times.Once);
        }

        [Test]
        public void WhenOnExceptionIsCalled_ThenTheInteralInterceptorIsCalled()
        {
            var exception = new Exception();
            _adapter.OnException(exception);

            _internalInterceptor.Verify(x => x.OnException(exception), Times.Once);
        }
    }
}
