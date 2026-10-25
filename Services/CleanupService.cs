using Microsoft.Extensions.DependencyInjection;

namespace SimpleChat.Services;

public class CleanupService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public CleanupService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public void Cleanup()
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            var users = userService.GetAll();
            Console.WriteLine($"[CLEANUP] Выполнена очистка. Всего пользователей: {users.Count}");
        }
    }
}