namespace Entities;

using System.Text.Json.Serialization;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    
    [JsonConstructor]
    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }
    private User(){}
    
    public List<Post> Posts { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
}