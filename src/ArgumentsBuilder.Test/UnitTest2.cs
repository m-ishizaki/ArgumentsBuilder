using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using Rksoftware.ArgumentsBuilder.Attributes;
using Rksoftware.ArgumentsBuilder.Hosts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentsBuilder.Test;

internal class UnitTest2
{
    [SetUp]
    public void Setup()
    {
    }

    record struct Arguments([Parameter] string? Aparam, [Parameter] string? Bparam, string? A, string? B, string? C, bool D, bool E) { }

    interface IService { }

    class PublicService : IService
    {
        public string Run(Arguments arguments) => arguments.ToString();
    }
    class NonPublicService : IService
    {
        string Run(Arguments arguments) => arguments.ToString();
    }

    [Test]
    public void Test1()
    {
        var args = new[] {
            "aparam",
            "-a",
            "aopt",
            "-d",
            "-b",
            "bopt",
            "bparam",
            "/c",
            "copt",
            };
        using IHost host = Host.CreateDefaultBuilder(args).ConfigureServices((_, services) => services.AddSingleton<IService, PublicService>()).Build();
        var rsult = host.ApplicationRun<IService, Arguments>(args);
        Assert.AreEqual(rsult, "Arguments { Aparam = aparam, Bparam = bparam, A = aopt, B = bopt, C = copt, D = True, E = False }");
    }

    [Test]
    public void Test2()
    {
        var args = new[] {
            "aparam",
            "-a",
            "aopt",
            "-d",
            "-b",
            "bopt",
            "bparam",
            "/c",
            "copt",
            };
        using IHost host = Host.CreateDefaultBuilder(args).ConfigureServices((_, services) => services.AddSingleton<IService, NonPublicService>()).Build();
        var rsult = host.ApplicationRun<IService, Arguments>(args);
        Assert.AreEqual(rsult, "Arguments { Aparam = aparam, Bparam = bparam, A = aopt, B = bopt, C = copt, D = True, E = False }");
    }

    [Test]
    public void Test3()
    {
        var args = new[] {
            "aparam",
            "-a",
            "aopt",
            "-d",
            "-b",
            "bopt",
            "bparam",
            "/c",
            "copt",
            };
        using IHost host = Host.CreateDefaultBuilder(args).ConfigureServices((_, services) => services.AddSingleton<IService, PublicService>()).Build();
        var rsult = host.ApplicationRun<IService, Arguments>(args, "run");
        Assert.AreEqual(rsult, "Arguments { Aparam = aparam, Bparam = bparam, A = aopt, B = bopt, C = copt, D = True, E = False }");
    }

    [Test]
    public void Test4()
    {
        var args = new[] {
            "aparam",
            "-a",
            "aopt",
            "-d",
            "-b",
            "bopt",
            "bparam",
            "/c",
            "copt",
            };
        using IHost host = Host.CreateDefaultBuilder(args).ConfigureServices((_, services) => services.AddSingleton<IService, NonPublicService>()).Build();
        var rsult = host.ApplicationRun<IService, Arguments>(args, "run");
        Assert.AreEqual(rsult, "Arguments { Aparam = aparam, Bparam = bparam, A = aopt, B = bopt, C = copt, D = True, E = False }");
    }

}
