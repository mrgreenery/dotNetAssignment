namespace ApiContract;

public class UserDto
{
    public required string UserName { get; set; }
    public int Id { get; set; }
    
    public List<PostDto>? Posts { get; set; }
    public List<CommentDto>? Comments { get; set; }
}