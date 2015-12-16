using LiberisLabs.DogStatsD.Interceptors.Tests.Helpers;
using NUnit.Framework;

namespace LiberisLabs.DogStatsD.Interceptors.Tests
{
    [TestFixture]
    public class StatNameCreatorTests
    {
        private StatNameCreator _creator;
        private string _result;

        [TestFixtureSetUp]
        public void Given()
        {
            _creator = new StatNameCreator();
        }

        [SetUp]
        public void WhenCreatingAStatName()
        {
            var methodInfo =
                new MethodInfoBuilder().WithNamespace("Namespace")
                    .WithClassName("ClassName")
                    .WithMethodName("MethodName")
                    .Create();

            _result = _creator.Create(methodInfo);
        }

        [Test]
        public void ThenTheResultIsCorrect()
        {
            Assert.That(_result, Is.EqualTo("namespace.classname.methodname"));
        }
    }
}
