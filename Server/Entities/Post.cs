using System.Text.Json.Serialization;

namespace Entities;

using System.Text.Json.Serialization;

public class Post
{
    [JsonConstructor]
    public Post() {} 

    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Body { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public int UserId { get; set; }
}
