using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView(IUserRepository userRepository)
{
    public async Task RunAsync()
    {
        //start menu
        Console.Clear();
        Console.WriteLine("----Create new user----");
        
        Console.Write("Enter username: ");
        string? username = Console.ReadLine();
        Console.Write("Enter password: ");
        string? password = Console.ReadLine();

        //sanity check
        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("Username cannot be empty.");
            return;
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Password cannot be empty.");
            return;
        }
        bool exists = userRepository
            .GetManyAsync()
            .Any(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));

        if (exists)
        {
            Console.WriteLine("Username already exists.");
            return;
        }

        //create
        var newUser = new User(username.Trim(), password);
        User created = await userRepository.AddAsync(newUser);

        Console.WriteLine($"User {created.Id} ({created.Username}) created.");
    }
    }