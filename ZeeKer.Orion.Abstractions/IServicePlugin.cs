using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ZeeKer.Orion.Abstractions;

public interface IServicePlugin : IPlugin
{
    void ConfigureServices(IServiceCollection services, IConfiguration config);
}