using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    public List<Post> Posts;

    public PostInMemoryRepository()
    {
        Posts = new List<Post>
        {
            new Post("Welcome", "Sharp your C's and net your dots", DateTime.UtcNow)
            {
                Id = 1,
                UserId = 1,
                Updated = DateTime.UtcNow
            },
            new Post("Jungle", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", DateTime.UtcNow)
            {
                Id = 2,
                UserId = 2,
                Updated = DateTime.UtcNow
            }
        };
    }

 
    public Task<Post> AddAsync(Post post)
    {
        post.Id = Posts.Any()
            ? Posts.Max(p => p.Id) + 1
            : 1;
        Posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
      Post? existingPost = Posts.SingleOrDefault(p=> p.Id==post.Id);
      if (existingPost is null)
      {
          throw new InvalidOperationException(
              $"Post with ID '{post.Id}' not found");
      }
      Posts.Remove(existingPost);
      Posts.Add(post);
      return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Post? postToRemove = Posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }

        Posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int id)
    {
        Post? post = Posts.SingleOrDefault(p => p.Id == id);
        if (post is null)
        {
            throw new InvalidOperationException(
                $"Post with ID '{id}' not found");
        }
        return Task.FromResult(post);
    }

    public IQueryable<Post> GetManyAsync()
    {
        return Posts.AsQueryable();
    }
}