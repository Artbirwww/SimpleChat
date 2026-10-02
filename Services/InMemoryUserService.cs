using SimpleChat.Models;

namespace SimpleChat.Services;

public class InMemoryUserService : IUserService
{
    private readonly List<User> _users = new();
    private int _nextId = 1;

    public List<User> GetAll()
    {
        return _users;
    }

    public User? GetById(int id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }

    public User Create(User user)
    {
        user.Id = _nextId++;
        user.CreatedAt = DateTime.UtcNow;
        user.LastActivityAt = DateTime.UtcNow; 
        
        _users.Add(user);
        return user;
    }
}