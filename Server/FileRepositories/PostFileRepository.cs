using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository: IPostRepository
{
    private readonly string _filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }
    public async Task<Post> AddAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(_filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        if (posts == null)
        {
            posts = new List<Post>();
        }
        
        int maxId = posts.Count > 0 ? posts.Max(p => p.Id) :0;
        post.Id = ++maxId;
        
        posts.Add(post);
        postsAsJson = JsonSerializer.Serialize(posts);
        
        await File.WriteAllTextAsync(_filePath, postsAsJson);
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(_filePath); 
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        
        int index = posts.FindIndex(p =>p.Id == post.Id);
        if (index != -1)
        {
            posts[index] = post;
            postsAsJson = JsonSerializer.Serialize(posts);
            await File.WriteAllTextAsync(_filePath, postsAsJson);
        }
    }

    public async Task DeleteAsync(int id)
    {
        string postsAsJson = await  File.ReadAllTextAsync(_filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        
        var toDelete = posts.Find(post => post.Id == id);
        if (toDelete != null)
        {
            posts.Remove(toDelete);
            postsAsJson = JsonSerializer.Serialize(posts);
            await File.WriteAllTextAsync(_filePath, postsAsJson);
        }
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        string postsAsJson = await  File.ReadAllTextAsync(_filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        
        return posts.Single(p => p.Id == id) ?? throw new InvalidOperationException();
    }

    public IQueryable<Post> GetManyAsync()
    {
        string postsAsJson = File.ReadAllTextAsync(_filePath).Result;
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        return posts.AsQueryable();
    }
}