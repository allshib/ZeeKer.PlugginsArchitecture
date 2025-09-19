using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ZeeKer.PlugginsArchitecture.Abstractions;

public interface IServicePlugin : IPlugin
{
    void ConfigureServices(IServiceCollection services, IConfiguration config);
}