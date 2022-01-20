using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rksoftware.ArgumentsBuilder.Hosts;

public static class ConfigurationExtendions
{
    public static IConfigurationBuilder AddCommandLineArguments<T>(this IConfigurationBuilder configurationBuilder, T arguments)
    {
        var type = typeof(T);
        var args =
        Enumerable.Concat(
        type.GetFields().Select(f => (f.Name, f.GetValue(arguments)?.ToString()))
        , type.GetProperties().Select(p => (p.Name, p.GetValue(arguments)?.ToString())))
            .GroupBy(kv => kv.Name).Select(kv => ($"/{kv.Key}", kv.First().Item2)).SelectMany(kv => new string?[] { kv.Item1, (kv.Item2) }).ToArray();
        
        return configurationBuilder.AddCommandLine(args);
    }

    public static IConfigurationBuilder AddCommandLineArguments<T>(this IConfigurationBuilder configurationBuilder, string[] args) where T:new() 
    {
        var arguments = ArgumentsBuilder.Parse<T>(args);

        return configurationBuilder.AddCommandLineArguments(arguments);

    }


}
