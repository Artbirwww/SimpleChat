using SimpleChat.Models;

namespace SimpleChat.Services;

public class UserStatsService : IUserStatsService
{
    private readonly IUserService _userService;

    public UserStatsService(IUserService userService)
    {
        _userService = userService;
    }

    public int GetTotalCount()
    {
        var users = _userService.GetAll();
        return users.Count;
    }

    public string GetLastUserName()
    {
        var users = _userService.GetAll();
        if (users.Count == 0)
        {
            return "Нет пользователей";
        }
        
        User lastUser = users[0];
        foreach (var u in users)
        {
            if (u.CreatedAt > lastUser.CreatedAt)
            {
                lastUser = u;
            }
        }
        return lastUser.UserName;
    }
}