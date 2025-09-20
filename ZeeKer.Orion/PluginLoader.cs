using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZeeKer.Orion.Abstractions;

namespace ZeeKer.Orion;

public static class PluginLoader
{
    public static IReadOnlyList<PluginDescriptor> LoadPlugins(
        IServiceCollection services,
        IConfiguration config,
        string pluginsDir,
        Version hostContractVersion)
    {
        Directory.CreateDirectory(pluginsDir);
        var descriptors = new List<PluginDescriptor>();

        foreach (var dll in Directory.EnumerateFiles(pluginsDir, "*.dll", SearchOption.AllDirectories))
        {
            var alc = new IsolatedPluginLoadContext(dll);
            var asm = alc.LoadFromAssemblyPath(dll);

            IEnumerable<Type> candidateTypes;

            try
            {
                candidateTypes = asm.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                candidateTypes = ex.Types.OfType<Type>();

                foreach (var loaderException in ex.LoaderExceptions)
                {
                    Console.Error.WriteLine(
                        $"Failed to load type from plugin assembly '{dll}': {loaderException.Message}");
                }
            }

            var pluginTypes = candidateTypes
                .Where(t => !t.IsAbstract && typeof(IPlugin).IsAssignableFrom(t))
                .ToArray();

            foreach (var type in pluginTypes)
            {
                var plugin = (IPlugin)Activator.CreateInstance(type)!;

                // Проверка совместимости
                if (plugin.MinHostVersion > hostContractVersion)
                    throw new InvalidOperationException(
                        $"Plugin {plugin.Id} requires host {plugin.MinHostVersion}, current {hostContractVersion}");

                // Регистрация сервисов, если нужно
                if (plugin is IServicePlugin sp)
                    sp.ConfigureServices(services, config);

                descriptors.Add(new PluginDescriptor
                {
                    Path = dll,
                    LoadContext = alc,
                    Instance = plugin
                });
            }
        }

        return descriptors;
    }
}