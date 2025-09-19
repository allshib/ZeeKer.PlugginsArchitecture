using Microsoft.Extensions.DependencyInjection;
using ZeeKer.PlugginsArchitecture.Abstractions;

namespace ZeeKer.PugginsArchetecture.HelloPluggin;


public class HelloPlugin : IApiPlugin
{
    public string Id => "HelloPlugin";

    public string Name => "HelloPlugin";

    public Version Version => new Version(1, 0, 0);

    public Version MinHostVersion => new Version(1, 0, 0);


    public void MapEndpoints(IEndpointGroup group)
    {
        group.MapGet("/hello", () => "Hello from HelloPlugin!");
    }

    //IPlugin IPlugin.Activate(IServiceCollection services, Type plugginType)
    //{
    //    var pluggin = services.AddTransient<>
    //}
}

