using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcCommentRepository : ICommentRepository
{
    private readonly AppContext ctx;

    public EfcCommentRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }


    public async Task<Comment> AddCommentAsync(Comment comment)
    {
        await ctx.Comments.AddAsync(comment);
        await ctx.SaveChangesAsync();
        return comment;
    }

    public async Task UpdateCommentAsync(Comment comment)
    {
        if (!(await ctx.Comments.AnyAsync(c => c.Id == comment.Id)))
        {
            throw new InvalidOperationException($"Comment not comment id {comment.Id} found");
        }
        ctx.Comments.Update(comment);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(int id)
    {
        Comment?  comment = await ctx.Comments.SingleOrDefaultAsync(c => c.Id == id);
        if (comment == null)
        {
            throw new InvalidOperationException($"Comment not comment id {id} found");
        }
        ctx.Comments.Remove(comment);
        await ctx.SaveChangesAsync();
    }

    public async Task<Comment> GetSingleCommentAsync(int id)
    {
        Comment? comment = await ctx.Comments.SingleOrDefaultAsync(c => c.Id == id);
        if (comment == null)
        {
            throw new InvalidOperationException($"Comment not comment id {id} found");
        }
        return comment;
    }

    public IQueryable<Comment> GetManyCommentsAsync()
    {
        return ctx.Comments.AsQueryable();
    }
}