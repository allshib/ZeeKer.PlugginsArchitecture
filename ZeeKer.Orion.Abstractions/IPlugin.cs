using Microsoft.Extensions.DependencyInjection;

namespace ZeeKer.Orion.Abstractions;

public interface IPlugin
{
    string Id { get; }
    string Name { get; }
    Version Version { get; } 
    Version MinHostVersion { get; }


    //IPlugin Activate(IServiceCollection services, Type plugginType)
    //{
    //    return (IPlugin)Activator.CreateInstance(plugginType)!;
    //}
}