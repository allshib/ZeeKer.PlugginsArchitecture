using Microsoft.Extensions.DependencyInjection;
using ZeeKer.Orion.Abstractions;

namespace ZeeKer.Orion.HelloPluggin;


public class HelloPlugin : IApiPlugin
{
    public string Id => "HelloPlugin";

    public string Name => "HelloPlugin";

    public Version Version => new Version(2, 0, 0);

    public Version MinHostVersion => new Version(1, 0, 0);


    public void MapEndpoints(IEndpointGroup group)
    {
        group.MapGet("/hello", () => "Hello from HelloPlugin V2!");
    }

    //IPlugin IPlugin.Activate(IServiceCollection services, Type plugginType)
    //{
    //    var pluggin = services.AddTransient<>
    //}
}

