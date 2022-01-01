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

        var parameterNames = ((IEnumerable<string>)(argumentsType.GetProperty(nameof(IArguments.ParameterNames))?.GetValue(null) ?? new string[0])).ToList();
        foreach (var o in options.Where(o => parameterNames.Contains(o.Key.Name)).ToArray()) options.Remove(o.Key);
        foreach (var o in switchs.Where(o => parameterNames.Contains(o.Key.Name)).ToArray()) options.Remove(o.Key);

        var constractor = argumentsType.GetConstructors().OrderByDescending(c => c.GetParameters().Length).FirstOrDefault();
        T arguments = constractor switch
        {
            null => new(),
            _ => ((Func<T>)(() =>
            {
                var constractorParameterLength = constractor.GetParameters().Length;
                var constractorParameters = constractor.GetParameters();
                var constractorParameterValues = constractorParameters.Select(p =>
                {
                    var name = p.Name ?? string.Empty;
                    if (parameterNames.Contains(name))
                    {
                        parameterNames.Remove(name);
                        var value = parameters.FirstOrDefault();
                        if (parameters.Count > 0) parameters.RemoveAt(0);
                        return value;
                    }

                    {
                        var option = options.FirstOrDefault(o => o.Key.Name == name);
                        if (!string.IsNullOrWhiteSpace(option.Key?.Name))
                        {
                            options.Remove(option.Key);
                            return option.Value;
                        }
                    }

                    {
                        var @switch = switchs.FirstOrDefault(o => o.Key.Name == name);
                        if (!string.IsNullOrWhiteSpace(@switch.Key?.Name))
                        {
                            switchs.Remove(@switch.Key);
                            return @switch.Value;
                        }
                    }

                    return (object?)null;
                }).ToArray();

                return (T)constractor.Invoke(constractorParameterValues);
            }))(),
        };

        foreach (var parameter in Enumerable.Zip(parameterNames, parameters)) argumentsType.GetProperty(parameter.First)?.SetValue(arguments, parameter.Second);
        foreach (var option in options) option.Key.SetValue(arguments, option.Value);
        foreach (var @switch in switchs) @switch.Key.SetValue(arguments, @switch.Value);

        return arguments;
    }
}

