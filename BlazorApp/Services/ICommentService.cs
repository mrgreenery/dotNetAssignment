using ApiContract;

namespace BlazorApp.Services;

public interface ICommentService
{
    public Task<CommentDto> AddComment(CreateCommentDto request);
    public Task UpdateComment(int id, UpdateCommentDto request);
    public Task<CommentDto> GetCommentAsync(int id);
    public Task<List<CommentDto>> GetCommentsAsync(int? userId, int? postId);
    public Task DeleteCommentAsync(int id);
    
}