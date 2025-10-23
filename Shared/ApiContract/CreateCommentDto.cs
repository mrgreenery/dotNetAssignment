namespace ApiContract;

public class CreateCommentDto
{
    public required String Body { get; set; }
    public required String Title { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
}
