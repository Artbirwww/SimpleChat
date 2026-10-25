using SimpleChat.Models;
using SimpleChat.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

namespace SimpleChat.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var usersGroup = app.MapGroup("/api/users");

        usersGroup.MapGet("/", (IUserService service) =>
        {
            var users = service.GetAll();
            return Results.Ok(users);
        });

        usersGroup.MapGet("/{id:int}", (int id, IUserService service) =>
        {
            var user = service.GetById(id);
            if (user == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(user);
        }).WithName("GetUserById");

        usersGroup.MapPost("/", (User user, IUserService service) =>
        {
            if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Email))
            {
                return Results.BadRequest("UserName и Email обязательны.");
            }

            var createdUser = service.Create(user);
            return Results.Created($"/api/users/{createdUser.Id}", createdUser);
        });

        usersGroup.MapGet("/page/{page=1}", (int page, IUserService service) =>
        {
            int pageSize = 5;
            var items = service.GetPaged(page, pageSize);
            var totalCount = service.GetAll().Count;

            var response = new
            {
                Page = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Items = items
            };
            return Results.Ok(response);
        });

        usersGroup.MapGet("/positive/{id:positiveInt}", (int id, IUserService service) =>
        {
            var user = service.GetById(id);
            if (user == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(user);
        });

        usersGroup.MapGet("/search", ([FromQuery] string? query, [FromQuery] int? year, [FromHeader(Name = "X-Client-Version")] string? clientVersion, IUserService service) =>
        {
            var users = service.GetAll();
            var filteredUsers = new List<User>();

            foreach (var u in users)
            {
                bool matchesQuery = true;
                bool matchesYear = true;

                if (string.IsNullOrEmpty(query) == false)
                {
                    if (u.UserName.Contains(query, StringComparison.OrdinalIgnoreCase) == false)
                    {
                        matchesQuery = false;
                    }
                }

                if (year.HasValue)
                {
                    if (u.CreatedAt.Year != year.Value)
                    {
                        matchesYear = false;
                    }
                }

                if (matchesQuery == true && matchesYear == true)
                {
                    filteredUsers.Add(u);
                }
            }

            string version = "unknown";
            if (clientVersion != null)
            {
                version = clientVersion;
            }

            var response = new
            {
                ClientVersion = version,
                Count = filteredUsers.Count,
                Items = filteredUsers
            };

            return Results.Ok(response);
        });

        usersGroup.MapGet("/{id:int}/link", (int id, IUserService service, LinkGenerator linkGenerator) =>
        {
            var user = service.GetById(id);
            if (user == null)
            {
                return Results.NotFound();
            }

            var url = linkGenerator.GetPathByRouteValues("GetUserById", new { id = id });
            return Results.Ok(new { UserId = id, Url = url });
        });

        // НОВОЕ: Ограничение маршрута по регулярному выражению (regex)
        usersGroup.MapGet("/slug/{slug:regex([a-z0-9-]+)}", (string slug, IUserService service) =>
        {
            var users = service.GetAll();
            foreach (var u in users)
            {
                string currentSlug = u.UserName.ToLower().Replace(" ", "-");
                if (currentSlug == slug)
                {
                    return Results.Ok(u);
                }
            }
            return Results.NotFound();
        });

        // НОВОЕ: Вложенная группа для административных операций
        var adminGroup = app.MapGroup("/api/admin");
        var adminUsers = adminGroup.MapGroup("/users");

        adminUsers.MapDelete("/{id:int}", (int id, IUserService service) =>
        {
            bool isDeleted = service.Delete(id);
            if (isDeleted == true)
            {
                return Results.NoContent();
            }
            return Results.NotFound();
        });
    }
}