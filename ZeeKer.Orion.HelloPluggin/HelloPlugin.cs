using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZeeKer.Orion.Abstractions;
using ZeeKer.Orion.Services;

namespace ZeeKer.Orion.HelloPluggin;


public class HelloPlugin : IApiPlugin
{
    public string Id => "HelloPlugin";

    public string Name => "HelloPlugin";

    public Version Version => new Version(3, 0, 0);

    public Version MinHostVersion => new Version(1, 0, 0);


    public void MapEndpoints(IEndpointGroup group)
    {
        group.MapGet("/hello", (IWriteTextService textService) => textService.WriteText());
    }
}

