using ZeeKer.PlugginsArchitecture.Abstractions;

namespace ZeeKer.PlugginsArchitecture.API.Services;

public static class PluginMapper
{
    public static void MapApiPlugins(
        IEndpointRouteBuilder aspnetEndpoints,
        IEnumerable<PluginDescriptor> plugins)
    {
        foreach (var pd in plugins)
        {
            if (pd.Instance is IApiPlugin api)
            {
                // Префикс: /plugins/{id}/v{major}
                var root = aspnetEndpoints.MapGroup($"/plugins/{api.Id}/v{api.Version.Major}");
                var adapter = new EndpointGroupAdapter(root);
                api.MapEndpoints(adapter);
            }
        }
    }
}

