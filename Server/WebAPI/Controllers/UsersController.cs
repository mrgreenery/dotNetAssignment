using System.Linq;
using ApiContract;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UsersController: ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        this._userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
    {
        await VerifyUserNameIsAvailableAsync(request.UserName);

        User user = new(request.UserName, request.Password);
        User created = await _userRepository.AddUserAsync(user);
        UserDto dto = new()
        {
            Id = created.Id,
            UserName = created.Username
        };
        return Created($"/users/{dto.Id}", dto); 
    }

    [HttpGet("{id}")]  
    public async Task<ActionResult<UserDto>> GetUser(int id)  
    {
        try
        {
            User user = await _userRepository.GetSingleUserAsync(id);  
            UserDto dto = new()
            {
                Id = user.Id,           
                UserName = user.Username
            };
            return Ok(dto);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetUsers([FromQuery] string? username)
    {
        try
        {
            //starting with all users
            var usersQuery = _userRepository.GetManyUsersAsync();
            
            //is username is provided...
            if (!string.IsNullOrEmpty(username))
            {
                usersQuery = usersQuery.Where(u => u.Username.ToLower().Contains(username.ToLower()));
            }
            
            //transform to dto and to list
            List<UserDto> userDtos = await usersQuery
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.Username
                })
                .ToListAsync<UserDto>();
            
            return Ok(userDtos);


        }
        catch (Exception e)
        {
         return StatusCode(500, e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(int id, [FromBody] UpdateUserDto request)
    {
        try
        {
            User existingUser = await _userRepository.GetSingleUserAsync(id);
        
            // FIRST: Verify username is available (if it's changing)
            if (!string.IsNullOrWhiteSpace(request.Username) && 
                existingUser.Username.ToLower() != request.Username.ToLower())
            {
                await VerifyUserNameIsAvailableAsync(request.Username);
            }
        
            // THEN: Update the properties
            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                existingUser.Username = request.Username;
            }

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                existingUser.Password = request.Password;
            }
        
            await _userRepository.UpdateUserAsync(existingUser);
            return NoContent();
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
    
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        try
        {
            // Step 1: Delete the user
            User user =  await _userRepository.GetSingleUserAsync(id);
            await _userRepository.DeleteUserAsync(id);
        
            // Step 2: Return success
            return NoContent(); // 204 No Content
        }
        catch (Exception e)
        {
            // Step 3: Handle errors
            return NotFound(e.Message);
        }
    }

    private async Task VerifyUserNameIsAvailableAsync(string userName)
    {
        bool exists = _userRepository
            .GetManyUsersAsync()
            .Any(u => u.Username.ToLower() == userName.ToLower());

        if (exists)
        {
            throw new InvalidOperationException($"User '{userName}' already exists");
        }
        
    }
}