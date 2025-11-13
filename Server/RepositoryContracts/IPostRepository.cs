using Entities;

namespace RepositoryContracts;

public interface IPostRepository
{
    Task<Post> AddPostAsync(Post post);
    Task UpdatePostAsync(Post post);
    Task DeletePostAsync(int id);
    Task<Post> GetSinglePostAsync(int id);
    IQueryable<Post> GetManyPostsAsync();


}