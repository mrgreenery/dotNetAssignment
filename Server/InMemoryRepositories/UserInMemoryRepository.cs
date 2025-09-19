using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository: IUserRepository
{
    public List<User> users;

    public UserInMemoryRepository()
    {
        users = new List<User>
        {
            new User("Henk", "welcome123") { Id = 1 },
            new User("Frits", "password") { Id = 2 },
            new User("Anneke", "wachtwoord") { Id = 3 },
        };
    }
    
    public Task<User> AddAsync(User user)
    {
        user.Id = users.Any()
            ? users.Max(x => x.Id) + 1
            : 1;
        users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        User? existingUser = users.SingleOrDefault(x => x.Id == user.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with id {user.Id} not found");
        }
        users.Remove(existingUser);
        users.Add(user);
        return Task.CompletedTask;;
    }

    public Task DeleteAsync(int id)
    {
        User? userToRemove = users.SingleOrDefault(x => x.Id == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException(
                $"User with id {id} not found");
        }
        users.Remove(userToRemove);
        return Task.CompletedTask;;
    }

    public Task<User> GetSingleAsync(int id)
    {
        User? user = users.SingleOrDefault(x => x.Id == id);
        if (user is null) 
        {
            throw new InvalidOperationException(
                $"User with id {id} not found");
        }
        return Task.FromResult(user);
    }

    public IQueryable<User> GetManyAsync()
    {
        return users.AsQueryable();
    }
}