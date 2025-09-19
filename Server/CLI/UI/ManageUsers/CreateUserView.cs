using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository _userRepository;

    public CreateUserView(IUserRepository userRepository)
    {
        this._userRepository = userRepository;
    }
    public async Task RunAsync()
    {
        Console.Clear();
        Console.WriteLine("----Create new user----");
        
        Console.Write("Enter username: ");
        string username = Console.ReadLine();
        Console.Write("Enter password: ");
        string password = Console.ReadLine();
        
        var newUser = new User(username, password);
        
        User created = await _userRepository.AddAsync(newUser);
        
        Console.WriteLine($"User {created.Id} created");
    }
    }