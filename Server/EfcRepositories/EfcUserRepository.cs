using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcUserRepository : IUserRepository
{
    private readonly AppContext ctx;
    
    public EfcUserRepository(AppContext ctx)
        {
        this.ctx = ctx;
        }


    public async Task<User> AddUserAsync(User user)
    {
        await ctx.Users.AddAsync(user);
        await ctx.SaveChangesAsync();
        return user;
    }

    public async Task UpdateUserAsync(User user)
    {
        if (!(await ctx.Users.AnyAsync(u => u.Id == user.Id)))
        {
            throw new InvalidOperationException($"User with user id {user.Id} not found");
        }
        ctx.Users.Update(user);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        User? existingUser = await ctx.Users.SingleOrDefaultAsync(u => u.Id == id);
        if (existingUser == null)
        {
            throw new InvalidOperationException($"User with user id {id} not found");
        }
        ctx.Users.Remove(existingUser);
        await ctx.SaveChangesAsync();
    }

    public async Task<User> GetSingleUserAsync(int id)
    {
        User? user = await ctx.Users.SingleOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            throw new InvalidOperationException($"User with user id {id} not found");
        }
        return user;
    }

    public IQueryable<User> GetManyUsersAsync()
    {
        return ctx.Users.AsQueryable();
    }
}