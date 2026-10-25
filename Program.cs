using SimpleChat.Endpoints;
using SimpleChat.Middleware;
using SimpleChat.Models;
using SimpleChat.Services;
using SimpleChat.Routing;
using Microsoft.AspNetCore.Routing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddSingleton<IUserService, InMemoryUserService>();
builder.Services.AddSingleton<IUserStatsService, UserStatsService>();

builder.Services.AddKeyedSingleton<IOperationLogger, ConsoleOperationLogger>("console");
builder.Services.AddKeyedSingleton<IOperationLogger, FileOperationLogger>("file");

builder.Services.AddSingleton<CleanupService>();

builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("positiveInt", typeof(PositiveIntRouteConstraint));
});

var app = builder.Build();

app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredKeyedService<IOperationLogger>("console");
    logger.Log($"Запрос: {context.Request.Method} {context.Request.Path}");
    
    await next();
    
    logger.Log($"Статус ответа: {context.Response.StatusCode}");
});

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapGet("/info", (Microsoft.Extensions.Options.IOptions<AppSettings> settings) =>
{
    var s = settings.Value;
    return $"Приложение: {s.AppName}, Версия: {s.Version}, Автор: {s.Author}";
});

app.MapGet("/stats", (IUserStatsService stats) =>
{
    int totalCount = stats.GetTotalCount();
    string lastUserName = stats.GetLastUserName();
    return $"Всего пользователей: {totalCount}. Последний пользователь: {lastUserName}";
});

app.MapGet("/log/console", ([FromKeyedServices("console")] IOperationLogger logger) =>
{
    logger.Log("Тестовое сообщение в консоль");
    return "Записано в консоль";
});

app.MapGet("/log/file", ([FromKeyedServices("file")] IOperationLogger logger) =>
{
    logger.Log("Тестовое сообщение в файл");
    return "Записано в файл";
});

app.MapGet("/cleanup", (CleanupService cleanup) =>
{
    cleanup.Cleanup();
    return "Очистка выполнена";
});

app.MapUserEndpoints();

app.MapGet("/", () => "Welcome to Simple Chat API. Use /api/users to interact.");

app.Run();