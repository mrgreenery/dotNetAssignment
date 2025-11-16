using ApiContract;
using BlazorApp.Services;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private IUserRepository _userRepository;

    public AuthController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("auth/login")]
    public async Task<ActionResult<UserDto>> ValidateUser([FromBody] UserLoginDto request)
    {
            //find user by username
            User? user = _userRepository.GetManyUsersAsync()
                .FirstOrDefault(u => u.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase));
            
            //if user does not exist, return error code
            if (user == null)
            {
                return Unauthorized("User not found");
            }
            
            //if password is incorrect, return error code
            if (!user.Password.Equals(request.Password))
            {
                return Unauthorized("Password incorrect");
            }

            //convert the user to a UserDto(so we don't send back the password or other sensitive info)
            UserDto dto = new()
            {
                Id = user.Id,
                UserName = user.Username,
            };
            //return the UserDto
            return Ok(dto);

    }
    
    
}