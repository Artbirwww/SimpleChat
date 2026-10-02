using SimpleChat.Endpoints;
using SimpleChat.Middleware;
using SimpleChat.Models;
using SimpleChat.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddSingleton<IUserService, InMemoryUserService>();

var app = builder.Build();


app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    await next();
    Console.WriteLine($"Response Status: {context.Response.StatusCode}");
});

app.UseMiddleware<ErrorHandlingMiddleware>();


app.MapGet("/info", (Microsoft.Extensions.Options.IOptions<AppSettings> settings) =>
{
    var s = settings.Value;
    return $"Приложение: {s.AppName}, Версия: {s.Version}, Автор: {s.Author}";
});

app.MapUserEndpoints();

app.MapGet("/", () => "Welcome to Simple Chat API. Use /api/users to interact.");

app.Run();