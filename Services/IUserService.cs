using SimpleChat.Models;

namespace SimpleChat.Services;

public interface IUserService
{
    List<User> GetAll();
    User? GetById(int id);
    User Create(User user);
}