using ApiContract;

namespace BlazorApp.Services;

public interface IPostService
{
    public Task<PostDto> AddPostAsync(CreatePostDto request);
    public Task UpdatePostAsync(int id, UpdatePostDto request);
    public Task<PostDto> GetPostAsync(int id);
    public Task<List<PostDto>> GetPostsAsync(string? title, int? userId);
    public Task DeletePostAsync(int id);
}