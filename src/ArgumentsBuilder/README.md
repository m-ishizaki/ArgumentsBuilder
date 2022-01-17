This library parses command line arguments for console applications.

## Usage

```cs
using IHost host = Host.CreateDefaultBuilder(args).ConfigureServices((_, services) => services.AddSingleton<IService, Service>()).Build();
var rsult = host.ApplicationRun<IService, Arguments>(args);

record struct Arguments([Parameter] string? Aparam, [Parameter] string? Bparam, string? A, string? B, string? C, bool D, bool E) { }

interface IService { }

class Service : IService
{
    public string Run(Arguments arguments) => arguments.ToString();
}
```
```
<command> aparam -a aopt -d -b bopt bparam /c copt
```

## Usage

```cs
using IHost host = Host.CreateDefaultBuilder(args).ConfigureServices((_, services) => services.AddSingleton<IService, Service>())
    .ConfigureAppConfiguration(config => config.AddCommandLineArguments<Arguments>(args))
    .Build();
var rsult = host.ApplicationRun<IService, Arguments>(args);

record struct Arguments([Parameter] string? Aparam, [Parameter] string? Bparam, string? A, string? B, string? C, bool D, bool E) { }

interface IService { }

class Service : IService
{
    public IConfiguration configuration;
    public Service(IConfiguration configuration) => this.configuration = configuration;
    public string Run(Arguments arguments) => arguments.ToString();
}
```
```
<command> aparam -a aopt -d -b bopt bparam /c copt
```