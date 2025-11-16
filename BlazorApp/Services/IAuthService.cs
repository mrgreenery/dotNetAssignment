using System.Security.Claims;
using Entities;

namespace BlazorApp.Services;

public interface IAuthService
{
    Task<User> ValidateUser(string username, string password);
    Task RegisterUser(User user);
}

