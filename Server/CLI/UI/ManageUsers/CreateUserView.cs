using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView(IUserRepository userRepository)
{
    public async Task RunAsync()
    {
        Console.Clear();
        Console.WriteLine("----Create new user----");
        
        Console.Write("Enter username: ");
        string username = Console.ReadLine();
        Console.Write("Enter password: ");
        string password = Console.ReadLine();
        
        var newUser = new User(username, password);
        
        User created = await userRepository.AddAsync(newUser);
        
        Console.WriteLine($"User {created.Id} created");
    }
    
    
}