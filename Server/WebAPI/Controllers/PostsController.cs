using ApiContract;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository _postRepository;

    public PostsController(IPostRepository postRepository)
    {
        this._postRepository = postRepository;
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> AddPost([FromBody] CreatePostDto request)
    {
        try
        {
            Post post = new Post
            {
                Title = request.Title,
                Body = request.Body,
                UserId = request.UserId,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };

            Post created = await _postRepository.AddPostAsync(post);

            PostDto dto = new PostDto
            {
                Id = created.Id,
                Title = created.Title,
                Body = created.Body,
                UserId = created.UserId,
                Created = created.Created,
                Updated = created.Updated
            };
            return Created($"/api/posts/{created.Id}", dto);

        }
        catch (Exception e)
        {
          return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetPost(
        [FromRoute] int id,
        [FromQuery] bool includeAuthor = false,
        [FromQuery] bool includeComments = false)
    {
        IQueryable<Post> query = _postRepository
            .GetManyPostsAsync()
            .Where(p => p.Id == id);
            
        if (includeAuthor)
        {
            query = query.Include(p => p.User);
        }
        
        if (includeComments)
        {
            query = query.Include(p => p.Comments);
        }

        PostDto? postDto = await query.Select(post => new PostDto()
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId,
                Author = includeAuthor
                    ? new UserDto
                    {
                        Id = post.User.Id,
                        UserName = post.User.Username
                    }
                    : null,
                Comments = includeComments
                    ? post.Comments.Select(c => new CommentDto
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Body = c.Body,
                        UserId = c.UserId
                    }).ToList()
                    : new()
            })
            .FirstOrDefaultAsync();

        return postDto == null ? NotFound() : Ok(postDto);
    }

    [HttpGet]
    public async Task<ActionResult<List<PostDto>>> GetPosts(
        [FromQuery] string? title,
        [FromQuery] int? userId)
    {
        try
        {
            var postsQuery = _postRepository.GetManyPostsAsync();

            if (!string.IsNullOrEmpty(title))
            {
                postsQuery = postsQuery.Where(p => p.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
            }

            if (userId.HasValue)
            {
                postsQuery = postsQuery.Where(p => p.UserId == userId.Value);
            }

            List<PostDto> postDtos = await postsQuery
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Body = p.Body,
                    UserId = p.UserId,
                    Created = p.Created,
                    Updated = p.Updated
                })
                .ToListAsync();

            return Ok(postDtos);
        }

        catch (Exception exception)
        {
           return StatusCode(500, exception.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePost(int id, [FromBody] UpdatePostDto request)
    {
        try
        {
            Post post = await _postRepository.GetSinglePostAsync(id);
    
            // Update title if provided
            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                post.Title = request.Title;
            }
    
            // Update body if provided
            if (!string.IsNullOrWhiteSpace(request.Body))
            {
                post.Body = request.Body;
            }
    
            // Always update the Updated timestamp
            post.Updated = DateTime.UtcNow;
    
            await _postRepository.UpdatePostAsync(post);
            return NoContent();
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePost(int id)
    {
        try
        {
            Post post = await _postRepository.GetSinglePostAsync(id);
            await _postRepository.DeletePostAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}