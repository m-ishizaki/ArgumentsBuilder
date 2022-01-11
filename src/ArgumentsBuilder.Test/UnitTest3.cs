using Microsoft.Extensions.Configuration;
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
internal class UnitTest3
{
    [SetUp]
    public void Setup()
    {
    }

    record struct Arguments([Parameter] string? Aparam, [Parameter] string? Bparam, string? A, string? B, string? C, bool D, bool E) { }

    interface IService { }

    class Service : IService
    {
        public IConfiguration configuration;
        public Service(IConfiguration configuration) => this.configuration = configuration;
        public string Run(Arguments arguments) => arguments.ToString();
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
        using IHost host = new HostBuilder().ConfigureServices((_, services) => services.AddSingleton<IService, Service>())
            .ConfigureAppConfiguration(config => config.AddCommandLineArguments<Arguments>(args))
            .Build();
        var config = (Microsoft.Extensions.Configuration.ConfigurationRoot)((Service)host.Services.GetService<IService>()!).configuration;
        var d = config.Providers.Skip(4).FirstOrDefault();
        var rsult = host.ApplicationRun<IService, Arguments>(args);
        Assert.AreEqual(rsult, "Arguments { Aparam = aparam, Bparam = bparam, A = aopt, B = bopt, C = copt, D = True, E = False }");
    }


}
