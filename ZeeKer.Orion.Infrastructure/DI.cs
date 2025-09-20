using Microsoft.Extensions.DependencyInjection;
using ZeeKer.Orion.Infrastructure.Services;
using ZeeKer.Orion.Services;

namespace ZeeKer.Orion.Infrastructure;


public static class DI
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IWriteTextService, WriteTextService>();
        return services;
    }

}

