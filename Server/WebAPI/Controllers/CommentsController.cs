using ApiContract;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using System.Linq;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;

    public CommentsController(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment([FromBody] CreateCommentDto request)
    {
        try
        {
            Comment comment = new Comment
            {
                Title = request.Title,
                Body = request.Body,
                UserId = request.UserId,
                PostId = request.PostId,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };

            Comment created = await _commentRepository.AddAsync(comment);

            CommentDto dto = new CommentDto
            {
                Id = created.Id,
                Title = created.Title,
                Body = created.Body,
                UserId = created.UserId,
                PostId = created.PostId,
                Created = created.Created,
                Updated = created.Updated
            };
            
            return Created($"/api/comments/{created.Id}", dto);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetComment(int id)
    {
        try
        {
            Comment comment = await _commentRepository.GetSingleAsync(id);
            CommentDto dto = new CommentDto
            {
                Id = comment.Id,
                Title = comment.Title,
                Body = comment.Body,
                UserId = comment.UserId,
                PostId = comment.PostId,
                Created = comment.Created,
                Updated = comment.Updated
            };

            return Ok(dto);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<CommentDto>>> GetComments(
        [FromQuery] int? userId,
        [FromQuery] int? postId)
    {
        try
        {
            var commentsQuery = _commentRepository.GetManyAsync();

            if (userId.HasValue)
            {
                commentsQuery = commentsQuery.Where(c => c.UserId == userId.Value);
            }

            if (postId.HasValue)
            {
                commentsQuery = commentsQuery.Where(c => c.PostId == postId.Value);
            }

            List<CommentDto> commentDtos = commentsQuery
                .AsEnumerable()
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Body = c.Body,
                    UserId = c.UserId,
                    PostId = c.PostId,
                    Created = c.Created,
                    Updated = c.Updated
                })
                .ToList();

            return Ok(commentDtos);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateComment(int id, [FromBody] UpdateCommentDto request)
    {
        try
        {
            Comment comment = await _commentRepository.GetSingleAsync(id);

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                comment.Title = request.Title;
            }

            if (!string.IsNullOrWhiteSpace(request.Body))
            {
                comment.Body = request.Body;
            }

            comment.Updated = DateTime.UtcNow;

            await _commentRepository.UpdateAsync(comment);
            return NoContent();
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteComment(int id)
    {
        try
        {
            Comment comment = await _commentRepository.GetSingleAsync(id);
            await _commentRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}