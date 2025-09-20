
using ZeeKer.Orion;
using ZeeKer.Orion.API.Services;

var builder = WebApplication.CreateBuilder(args);

var hostContractVersion = new Version(1, 0, 0);

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var plugins = PluginLoader.LoadPlugins(
    builder.Services,
    builder.Configuration,
    Path.Combine(AppContext.BaseDirectory, "plugins"),
    hostContractVersion);


//foreach (var p in plugins.Select(p => p.Instance))
//{
//    if (p is IAuthorizationContributor auth)
//        builder.Services.AddAuthorization(o => auth.Contribute(o));

//    if (p is IOperationalContributor op)
//        builder.Services.AddHealthChecks().Also(b => op.ContributeHealthChecks(b));
//}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Ok("Host OK"));

PluginMapper.MapApiPlugins(app, plugins);

//// Healthchecks
//app.MapHealthChecks("/health");

app.UseAuthorization();

app.Run();
