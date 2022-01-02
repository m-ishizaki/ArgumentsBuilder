using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rksoftware.ArgumentsBuilder.Hosts;

public static class HostExtensions
{
    public static object? ApplicationRun<TService, TArguments>(this IHost host, string[] args, string startupMethod) where TArguments : new()
    {
        var service = host.Services.GetService<TService>();
        var serviceType = service!.GetType();
        var method =
            serviceType.GetMethod(startupMethod, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
            ?? serviceType.GetMethod(startupMethod, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
        return method!.Invoke(service, new object?[] { ArgumentsBuilder.Parse<TArguments>(args) });
    }

    public static object? ApplicationRun<TService, TArguments>(this IHost host, string[] args) where TArguments : new()
    {
        var service = host.Services.GetService<TService>();
        var serviceType = service!.GetType();
        var method =
            Enumerable.Concat(
                serviceType.GetMethods(BindingFlags.Public | BindingFlags.Instance),
                serviceType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            )
            .FirstOrDefault(m => m.GetParameters().Select(p=>p.ParameterType).SequenceEqual(new[] { typeof(TArguments) }));
        return method!.Invoke(service, new object?[] { ArgumentsBuilder.Parse<TArguments>(args) });
    }
}

