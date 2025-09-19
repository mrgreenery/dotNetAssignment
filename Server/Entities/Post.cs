namespace Entities;

public class Post(string title, string body, DateTime createdDate)
{
    public int Id { get; set; }
    public string Title { get; set; } = title;
    public string Body { get; set; } = body;
    public DateTime Created { get; set; } = createdDate.ToLocalTime();
    public DateTime Updated { get; set; } = createdDate.ToLocalTime();
    public int UserId { get; set; }
}