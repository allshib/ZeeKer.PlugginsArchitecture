using ZeeKer.Orion.Abstractions;

namespace ZeeKer.Orion.API.Services;

public sealed class EndpointGroupAdapter : IEndpointGroup
{
    private readonly RouteGroupBuilder group;
    public EndpointGroupAdapter(RouteGroupBuilder group) => this.group = group;

    public IEndpointGroup MapGet(string pattern, Delegate handler)
    {
        group.MapGet(pattern, handler);
        return this;
    }

    public IEndpointGroup MapPost(string pattern, Delegate handler)
    {
        group.MapPost(pattern, handler);
        return this;
    }

    public IEndpointGroup RequireAuthorization(params string[] policies)
    {
        group.RequireAuthorization(policies);
        return this;
    }
}