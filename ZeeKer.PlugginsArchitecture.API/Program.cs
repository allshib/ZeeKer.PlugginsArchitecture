
using ZeeKer.PlugginsArchitecture;
using ZeeKer.PlugginsArchitecture.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Версия контракта (меняйте при breaking changes в Abstractions)
var hostContractVersion = new Version(1, 0, 0);

// Базовые сервисы приложения...
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Грузим плагины (services stage)
var plugins = PluginLoader.LoadPlugins(
    builder.Services,
    builder.Configuration,
    Path.Combine(AppContext.BaseDirectory, "plugins"),
    hostContractVersion);

//// Подключаем вклад плагинов в авторизацию/операции
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

// Мапим основные эндпойнты ядра...
app.MapGet("/", () => Results.Ok("Host OK"));

// Мапим эндпойнты плагинов
PlugginMapper.MapApiPlugins(app, plugins);

//// Healthchecks
//app.MapHealthChecks("/health");

// Авторизация/аутентификация по месту
app.UseAuthorization();

app.Run();
