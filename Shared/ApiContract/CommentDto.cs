namespace ApiContract;

public class CommentDto
{
    public int Id { get; set; }
    public required String Body { get; set; }
    public required String Title { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
}