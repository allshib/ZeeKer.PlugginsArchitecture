namespace ZeeKer.Orion.Abstractions;

public sealed class PluginDescriptor
{
    public required string Path { get; init; }
    public required IsolatedPluginLoadContext LoadContext { get; init; }
    public required IPlugin Instance { get; init; }
}