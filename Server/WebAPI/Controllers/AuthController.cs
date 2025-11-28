using ApiContract;
using BlazorApp.Services;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private IUserRepository _userRepository;

    public AuthController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> ValidateUser([FromBody] UserLoginDto request)
    {

        Console.WriteLine($"Login attempt for username: '{request.Username}'"); //testing
        var allUsers = _userRepository.GetManyUsersAsync().ToList(); 
        Console.WriteLine($"Total users in repository: {allUsers.Count}");
        foreach (var u in allUsers)
        {
            Console.WriteLine($"  User: '{u.Username}'");
        }
        
        //find user by username
        
        User? user = await _userRepository.GetManyUsersAsync()
            .FirstOrDefaultAsync(u => u.Username.ToLower() == request.Username.ToLower());
            
            
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