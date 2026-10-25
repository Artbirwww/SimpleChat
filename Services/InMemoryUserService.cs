using SimpleChat.Models;

namespace SimpleChat.Services;

public class InMemoryUserService : IUserService
{
    private readonly List<User> _users = new List<User>();
    private int _nextId = 1;

    public InMemoryUserService()
    {
        // ==========================================================
        // НАЧАЛО БЛОКА СИДИРОВАНИЯ (SEED DATA)
        // Этот блок можно полностью удалить, когда перейдете на реальную БД.
        // ==========================================================
        
        SeedUser("Ivan", "ivan@mail.ru");
        SeedUser("Anna", "anna@yandex.ru");
        SeedUser("Petr", "petr@mail.ru");
        SeedUser("Elena", "elena@gmail.com");
        SeedUser("John", "john@example.com");
        SeedUser("Alice", "alice@example.com");
        SeedUser("Maria", "maria@mail.ru");
        SeedUser("Sergey", "sergey@yandex.ru");
        SeedUser("Olga", "olga@gmail.com");
        SeedUser("Dmitry", "dmitry@mail.ru");
        SeedUser("Ekaterina", "ekaterina@test.com");
        SeedUser("Michael", "michael@example.com");
        SeedUser("Julia", "julia@mail.ru");
        SeedUser("Alexey", "alexey@yandex.ru");
        SeedUser("Tatiana", "tatiana@gmail.com");
        SeedUser("Andrey", "andrey@mail.ru");
        SeedUser("Svetlana", "svetlana@test.com");
        SeedUser("Maxim", "maxim@example.com");
        SeedUser("Irina", "irina@mail.ru");
        SeedUser("Oleg", "oleg@yandex.ru");
        SeedUser("Natalia", "natalia@gmail.com");
        SeedUser("Pavel", "pavel@mail.ru");
        SeedUser("Anastasia", "anastasia@test.com");
        SeedUser("Roman", "roman@example.com");
        SeedUser("Victoria", "victoria@mail.ru");
        SeedUser("Denis", "denis@yandex.ru");
        SeedUser("Ksenia", "ksenia@gmail.com");
        SeedUser("Artem", "artem@mail.ru");
        SeedUser("Alina", "alina@test.com");
        SeedUser("Nikita", "nikita@example.com");
        SeedUser("Yulia", "yulia@mail.ru");
        SeedUser("Kirill", "kirill@yandex.ru");
        SeedUser("Sofia", "sofia@gmail.com");
        SeedUser("Egor", "egor@mail.ru");
        SeedUser("Valeria", "valeria@test.com");
        SeedUser("Vladislav", "vladislav@example.com");
        SeedUser("Daria", "daria@mail.ru");
        SeedUser("Anton", "anton@yandex.ru");
        SeedUser("Polina", "polina@gmail.com");
        SeedUser("Gleb", "gleb@mail.ru");

        // ==========================================================
        // КОНЕЦ БЛОКА СИДИРОВАНИЯ (SEED DATA)
        // ==========================================================
    }

    // ==========================================================
    // НАЧАЛО БЛОКА СИДИРОВАНИЯ (Вспомогательный метод)
    // Этот метод можно удалить вместе с вызовами выше.
    // ==========================================================
    private void SeedUser(string name, string email)
    {
        User newUser = new User();
        newUser.UserName = name;
        newUser.Email = email;
        Create(newUser);
    }
    // ==========================================================
    // КОНЕЦ БЛОКА СИДИРОВАНИЯ
    // ==========================================================

    public List<User> GetAll()
    {
        return _users;
    }

    public User? GetById(int id)
    {
        foreach (var u in _users)
        {
            if (u.Id == id)
            {
                return u;
            }
        }
        return null;
    }

    public User Create(User user)
    {
        user.Id = _nextId;
        _nextId = _nextId + 1;
        user.CreatedAt = DateTime.UtcNow;
        user.LastActivityAt = DateTime.UtcNow; 
        
        _users.Add(user);
        return user;
    }

    public List<User> GetPaged(int page, int pageSize)
    {
        int skipCount = (page - 1) * pageSize;
        return _users.Skip(skipCount).Take(pageSize).ToList();
    }

    public bool Delete(int id)
    {
        for (int i = 0; i < _users.Count; i++)
        {
            if (_users[i].Id == id)
            {
                _users.RemoveAt(i);
                return true;
            }
        }
        return false;
    }
}