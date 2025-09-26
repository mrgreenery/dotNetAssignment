using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository: IUserRepository
{
    private readonly string _filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }
    public async Task<User> AddAsync(User user)
    {
        string usersAsJson = await File.ReadAllTextAsync(_filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        
        bool exists = users.Any(u => string.Equals(u.Username, user.Username, StringComparison.OrdinalIgnoreCase));
        if (exists) 
        {
            throw new InvalidOperationException($"Username '{user.Username}' already exists."); 
        }

        int maxId = users.Count > 0 ? users.Max(c => c.Id) : 0;
        user.Id = maxId + 1;
        
        users.Add(user);
        usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(_filePath, usersAsJson);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        string  usersAsJson = await File.ReadAllTextAsync(_filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        
        int index = users.FindIndex(c => c.Id == user.Id);
        if (index != -1)
        {
            users[index] = user;
            usersAsJson = JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(_filePath, JsonSerializer.Serialize(users));
        }
    }

    public async Task DeleteAsync(int id)
    {
        string usersAsJson = await File.ReadAllTextAsync(_filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        
        var toDelete = users.Find(c => c.Id == id);
        if (toDelete != null)
        {
            users.Remove(toDelete);
            usersAsJson = JsonSerializer.Serialize(users);
            await File.WriteAllTextAsync(_filePath, usersAsJson);
        }
    }

    public async Task<User> GetSingleAsync(int id)
    {
        string usersAsJson = await File.ReadAllTextAsync(_filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        
        return users.Single(c => c.Id == id);
    }

    public IQueryable<User> GetManyAsync()
    {
        string  usersAsJson = File.ReadAllText(_filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        return users.AsQueryable();
    }
}