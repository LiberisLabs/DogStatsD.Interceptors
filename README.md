# DogStatsD.Interceptors

A castle proxy interceptor that branches out in to multiple interceptors to push metrics back to DataDog via DogStatsD.

[![install from nuget](http://img.shields.io/nuget/v/DogStatsD.Interceptors.svg?style=flat-square)](https://www.nuget.org/packages/DogStatsD.Interceptors)
[![downloads](http://img.shields.io/nuget/dt/DogStatsD.Interceptors.svg?style=flat-square)](https://www.nuget.org/packages/DogStatsD.Interceptors)
[![Build status](https://ci.appveyor.com/api/projects/status/7c3jrhlnp0bvph95/branch/master?svg=true)](https://ci.appveyor.com/project/Liberis/dogstatsd-interceptors/branch/master)

## Getting Started

DogStatsD.Interceptors is split in to 2 packages, `DogStatsD.Interceptors` and `DogStatsD.Interceptors.Annotations`. `DogStatsD.Interceptors` is the core implementation while `DogStatsD.Interceptors.Annotations` contains only Attributes which act as method markers.

DogStatsD.Interceptors can be installed via the package manager console by executing the following commandlet:

```powershell
PM> Install-Package DogStatsD.Interceptors
```

Once the package is installed, you need to setup the `DogStatsD` client for C#. Call the Configure method on the `DogStatsd` object as follows:

```csharp

var dogstatsdConfig = new StatsdConfig
{
    StatsdServerName = "127.0.0.1",
    StatsdPort = 8125, // Optional; default is 8125
    Prefix = "myApp" // Optional; by default no prefix will be prepended
};

StatsdClient.DogStatsd.Configure(dogstatsdConfig);
```

For more information on configuring DogStatsD client see the [DataDogs GitHub Page](https://github.com/DataDog/dogstatsd-csharp-client).

DogStatsD.Interceptors can then be plumbed in to any IoC container of choice.

### Registering Interceptors

#### Autofac

To use interceptors with Autofac we need to install the `Autofac.Extras.DynamicProxy2` NuGet package:

```powershell
PM> Install-Package Autofac.Extras.DynamicProxy2
```

The DogStatsdInterceptor will then need to be registered within the application's container builder:

```csharp
var builder = new ContainerBuilder();

builder.RegisterType<DogStatsdInterceptor>();
```

After the interceptor has been registered, the container needs to know what interfaces to proxy and apply the interceptor to. This is by calling `InterceptedBy` and then `EnableInterfaceInterceptors`
```csharp
builder.RegisterType<Service>()
    .InterceptedBy(typeof(DogStatsdInterceptor))
    .As<IService>()
    .EnableInterfaceInterceptors();
```

Once the container is built and the object is resolved, it will be proxied via the `DogStatsdInterceptor`.

For more detailed setups of interceptors see the [register-interceptors](http://docs.autofac.org/en/latest/advanced/interceptors.html#register-interceptors) within the Autofac documentation.

#### Other IoC

Most IoCs provide support for interceptors, please refer back to their documentation and maybe submit us a pull request to help others

## Usage

Currently DogStatsD.Interceptors supports 2 metrics:-

- Instrument
- Time

### Instrument

To Instrument a method, apply the `InstrumentAttribute` to the method either on the interface or the target type:

```csharp
public interface IService
{
    [Instrument]
    void InterfaceMethod();
}

public class Service : IService
{
    [Instrument]
    void Method();
}

```

When the method is called it will now push attempts, successes, errors and cancellations to DataDog in the following format:

`{stat-name}.{action}`

where `stat-name` is ExampleNamespace.ClassName.MethodName and `action` is attempt, success, error, canceled.

### Time

To Time a method, apply the `TimeAttribute` to the method either on the interface or the target type:

```csharp
public interface IService
{
    [Time]
    void InterfaceMethod();
}

public class Service : IService
{
    [Time]
    void Method();
}

```

When the method is called it will now push the time took to complete back to DataDog with an ExampleNamespace.ClassName.MethodName convention of a stat name.

This also supports methods that return Task objects, where the time taken is after the task has completed.

## Contribute

1. Fork
1. Hack!
1. Pull Request
