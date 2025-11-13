using ApiContract;

namespace BlazorApp.Services;

public interface IUserService
{
    public Task<UserDto> AddUserAsync(CreateUserDto request);
    public Task<UserDto> UpdateUserAsync(int id, UpdateUserDto request);
    
}