using System;
using System.Reflection;
using Moq;

namespace LiberisLabs.DogStatsD.Interceptors.Tests.Helpers
{
    public class MethodInfoBuilder
    {
        private string _namespace;
        private string _className;
        private string _methodName;

        public MethodInfoBuilder WithNamespace(string @namespace)
        {
            _namespace = @namespace;

            return this;
        }

        public MethodInfoBuilder WithClassName(string className)
        {
            _className = className;

            return this;
        }


        public MethodInfoBuilder WithMethodName(string methodName)
        {
            _methodName = methodName;

            return this;
        }

        public MethodInfo Create()
        {
            var type = new Mock<Type>();
            var fullName = $"{_namespace}.{_className}";
            type.Setup(x => x.FullName)
                .Returns(fullName);

            var methodInfo = new Mock<MethodInfo>();
            methodInfo.Setup(x => x.Name)
                .Returns(_methodName);
            methodInfo.Setup(x => x.ReflectedType)
                .Returns(type.Object);

            return methodInfo.Object;
        }
    }
}
