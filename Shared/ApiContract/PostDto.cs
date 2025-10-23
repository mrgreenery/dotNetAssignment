namespace ApiContract;

public class PostDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Body { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public int UserId { get; set; }
}