using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository: IUserRepository
{
    public List<User> Users;

    public UserInMemoryRepository()
    {
        Users = new List<User>
        {
            new User("Henk", "welcome123") { Id = 1 },
            new User("Frits", "password") { Id = 2 },
            new User("Anneke", "wachtwoord") { Id = 3 },
        };
    }
    
    public Task<User> AddAsync(User user)
    {
        user.Id = Users.Any()
            ? Users.Max(x => x.Id) + 1
            : 1;
        Users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        User? existingUser = Users.SingleOrDefault(x => x.Id == user.Id);
        if (existingUser is null)
        {
            throw new InvalidOperationException(
                $"User with id {user.Id} not found");
        }
        Users.Remove(existingUser);
        Users.Add(user);
        return Task.CompletedTask;;
    }

    public Task DeleteAsync(int id)
    {
        User? userToRemove = Users.SingleOrDefault(x => x.Id == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException(
                $"User with id {id} not found");
        }
        Users.Remove(userToRemove);
        return Task.CompletedTask;;
    }

    public Task<User> GetSingleAsync(int id)
    {
        User? user = Users.SingleOrDefault(x => x.Id == id);
        if (user is null) 
        {
            throw new InvalidOperationException(
                $"User with id {id} not found");
        }
        return Task.FromResult(user);
    }

    public IQueryable<User> GetManyAsync()
    {
        return Users.AsQueryable();
    }
}