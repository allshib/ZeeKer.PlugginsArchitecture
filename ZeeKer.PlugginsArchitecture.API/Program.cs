
using ZeeKer.PlugginsArchitecture;
using ZeeKer.PlugginsArchitecture.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Âåðñèÿ êîíòðàêòà (ìåíÿéòå ïðè breaking changes â Abstractions)
var hostContractVersion = new Version(1, 0, 0);

// Áàçîâûå ñåðâèñû ïðèëîæåíèÿ...
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Ãðóçèì ïëàãèíû (services stage)
var plugins = PluginLoader.LoadPlugins(
    builder.Services,
    builder.Configuration,
    Path.Combine(AppContext.BaseDirectory, "plugins"),
    hostContractVersion);

PluginMapper.MapApiPlugins(app, plugins);
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

// Ìàïèì îñíîâíûå ýíäïîéíòû ÿäðà...
app.MapGet("/", () => Results.Ok("Host OK"));

// Ìàïèì ýíäïîéíòû ïëàãèíîâ
PlugginMapper.MapApiPlugins(app, plugins);

//// Healthchecks
//app.MapHealthChecks("/health");

// Àâòîðèçàöèÿ/àóòåíòèôèêàöèÿ ïî ìåñòó
app.UseAuthorization();

app.Run();
