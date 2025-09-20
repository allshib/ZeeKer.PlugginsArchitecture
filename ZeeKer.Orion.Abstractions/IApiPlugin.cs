namespace ZeeKer.Orion.Abstractions;


public interface IEndpointGroup
{
    IEndpointGroup MapGet(string pattern, Delegate handler);
    IEndpointGroup MapPost(string pattern, Delegate handler);
    IEndpointGroup RequireAuthorization(params string[] policies);
}

public interface IApiPlugin : IPlugin
{
    void MapEndpoints(IEndpointGroup group);
}