using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{ 
    private readonly string filePath = "comments.json";  //define the file path
    
    public CommentFileRepository() //constructor ensures there actually is a file. If null, a new file is created with empty list
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");// contents is empty collection in json
        }
    }
    public async Task<Comment> AddAsync(Comment comment) 
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath); //read content from the file in json
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!; //json deserialized into list of comments

        if (comments == null) //check if there is a list, if not, create a new list
        {
            comments = new List<Comment>();
        }

        int maxId = comments.Count > 0 ? comments.Max(c => c.Id) : 0; //calculate the next ID to use
        comment.Id = maxId + 1; //set ID
        
        comments.Add(comment); //Add comment to the list
        commentsAsJson = JsonSerializer.Serialize(comments); // serialize list to json
        
        await File.WriteAllTextAsync(filePath, commentsAsJson); //write the jason back to the file
        return comment; //return the finalized comment that now has an ID
    }

    public async Task UpdateAsync(Comment comment)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath); 
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        int index = comments.FindIndex(c => c.Id == comment.Id);
        if (index >= 0)
        {
            comments[index] = comment;
            commentsAsJson = JsonSerializer.Serialize(comments);
            await File.WriteAllTextAsync(filePath, commentsAsJson); 
        }
    }
    
    public async Task DeleteAsync(int id)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath); 
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        
        var toDelete = comments.Find(c => c.Id == id); //find comment id and mark toDelete
        if (toDelete != null)
        {
            comments.Remove(toDelete);
            commentsAsJson = JsonSerializer.Serialize(comments); 
            await File.WriteAllTextAsync(filePath, commentsAsJson); 
        }
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        return comments.Single(c => c.Id == id);
    }

    public IQueryable<Comment> GetManyAsync()
    {
        string commentsAsJson = File.ReadAllTextAsync(filePath).Result; 
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!; 
        return comments.AsQueryable();
    }
}