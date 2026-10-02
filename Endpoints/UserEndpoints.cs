using SimpleChat.Models;
using SimpleChat.Services;

namespace SimpleChat.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("/api/users", (IUserService service) =>
        {
            var users = service.GetAll();
            return Results.Ok(users);
        });

        app.MapGet("/api/users/{id}", (int id, IUserService service) =>
        {
            var user = service.GetById(id);
            return user is null ? Results.NotFound() : Results.Ok(user);
        });

        app.MapPost("/api/users", (User user, IUserService service) =>
        {
            if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Email))
            {
                return Results.BadRequest("UserName и Email обязательны.");
            }

            var createdUser = service.Create(user);
            return Results.Created($"/api/users/{createdUser.Id}", createdUser);
        });
    }
}