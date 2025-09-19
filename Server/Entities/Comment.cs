namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public String Body { get; set; }
    public String Title { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
}