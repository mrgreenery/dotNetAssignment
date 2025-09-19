using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository: ICommentRepository
{
    public List<Comment> Comments;

    public CommentInMemoryRepository()
    {
        Comments = new List<Comment>();
    }
    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = Comments.Any()
            ? Comments.Max(c => c.Id) + 1
            : 1;
        Comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        Comment? existingComment = Comments.SingleOrDefault(c=> c.Id==comment.Id);
        if (existingComment is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID {comment.Id} was not found");
        }
        Comments.Remove(existingComment);
        Comments.Add(comment);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Comment? commentToRemove = Comments.SingleOrDefault(c => c.Id==id);
        if (commentToRemove is null)
        {
            throw new InvalidOperationException(
                $"Comment with ID {id} was not found");
        }
        Comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int id)
    {
        Comment? comment = Comments.SingleOrDefault(c => c.Id==id);
        if (comment is null) 
        { 
            throw new InvalidOperationException(
                $"Comment with ID {id} was not found");
        }
        return Task.FromResult(comment);
    }

    public IQueryable<Comment> GetManyAsync()
    {
        return Comments.AsQueryable();
    }
}