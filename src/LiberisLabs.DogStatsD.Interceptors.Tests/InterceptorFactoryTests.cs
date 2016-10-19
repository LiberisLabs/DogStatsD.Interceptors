using System;
using System.Collections.Generic;
using System.Reflection;
using LiberisLabs.DogStatsD.Interceptors.TaskInterceptors;
using Moq;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests
{
    [TestFixture]
    public class InterceptorFactoryTests
    {
        private InterceptorFactory _factory;
        private ICollection<ITaskInterceptor> _result;
        private ITaskInterceptor _validInterceptor1;
        private ITaskInterceptor _validInterceptor2;
        private MethodInfo _methodInvocationTarget;
        private MethodInfo _methodInfo;

        [OneTimeSetUp]
        public void GivenInterceptorFactoryWithSomeInterceptors()
        {
            string statName = Guid.NewGuid().ToString();
            _methodInfo = Mock.Of<MethodInfo>();
            _methodInvocationTarget = Mock.Of<MethodInfo>();

            var statNameCreator = new Mock<IStatNameCreator>();
            statNameCreator.Setup(x => x.Create(_methodInvocationTarget))
                .Returns(statName);

            var repo = new Mock<IInterceptorRepository>();

            _validInterceptor1 = CreateInterceptor(true);
            _validInterceptor2 = CreateInterceptor(true);
            var invalidInterceptor = CreateInterceptor(false);

            repo.Setup(x => x.GetAll(statName))
                .Returns(new List<ITaskInterceptor>()
                {
                    _validInterceptor1, invalidInterceptor, _validInterceptor2
                });

            _factory = new InterceptorFactory(repo.Object, statNameCreator.Object);
        }

        [SetUp]
        public void When()
        {
            _result = _factory.CreateInterceptors(_methodInfo, _methodInvocationTarget);
        }

        [Test]
        public void ThenOnlyValidInterceptorsAreReturned()
        {
            Assert.That(_result, Is.EquivalentTo(new[] {_validInterceptor1, _validInterceptor2}));
        }

        private ITaskInterceptor CreateInterceptor(bool canHandle)
        {
            var interceptor = new Mock<ITaskInterceptor>();

            interceptor.Setup(x => x.CanIntercept(_methodInfo, _methodInvocationTarget))
                .Returns(canHandle);

            return interceptor.Object;
        }
    }
}
