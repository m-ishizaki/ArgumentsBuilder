using Rksoftware.ArgumentsBuilder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rksoftware.ArgumentsBuilder;

public static class ArgumentsBuilder
{
    public static T Parse<T>(string[] args) where T : IArguments, new()
    {
        var argumentsType = typeof(T);
        List<string> parameters = new();
        Dictionary<PropertyInfo, string> options = new();
        Dictionary<PropertyInfo, bool> switchs = new();
        PropertyInfo? optionProperty = default;

        foreach (var arg in args.Select(arg => arg ?? string.Empty))
        {
            {
                var pre = arg.First();
                if (new[] { '-', '/' }.Contains(pre))
                {
                    var optionName = new string(arg.SkipWhile(c => c == pre).ToArray());
                    optionProperty = argumentsType.GetProperty(optionName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (optionProperty == null) continue;

                    if (optionProperty.PropertyType == typeof(bool))
                    {
                        switchs.Add(optionProperty, true);
                        optionProperty = null;
                        continue;
                    }

                    options.Add(optionProperty, "");
                    continue;
                }
            }

            if (optionProperty == null)
            {
                parameters.Add(arg);
                continue;
            }

            options[optionProperty] = arg;
            optionProperty = null;
        }

        var arguments = new T();
        foreach (var parameter in Enumerable.Zip(arguments.ParameterNames, parameters)) argumentsType.GetProperty(parameter.First)?.SetValue(arguments, parameter.Second);
        foreach (var option in options) option.Key.SetValue(arguments, option.Value);
        foreach (var @switch in switchs) @switch.Key.SetValue(arguments, @switch.Value);

        return arguments;
    }
}

