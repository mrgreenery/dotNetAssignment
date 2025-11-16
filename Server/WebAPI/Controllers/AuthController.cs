using ApiContract;
using BlazorApp.Services;
using Entities;
using Microsoft.AspNetCore.Mvc;
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

    [HttpPost("{auth/login}")]
    public async Task<ActionResult<UserLoginDto>> LoginUser([FromBody] UserLoginDto request)
    {
        try
        {
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        
    }
    
    
}