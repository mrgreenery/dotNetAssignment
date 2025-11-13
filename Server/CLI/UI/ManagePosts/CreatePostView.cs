using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView(IPostRepository postRepository, IUserRepository userRepository)
{
    public async Task RunAsync()
    {
        Console.Clear();
        Console.WriteLine("----Create new post----");
        
        Console.Write("What is the title of you post: ");
        string? title = Console.ReadLine();
        
        Console.Write("Write your post: ");
        string? body = Console.ReadLine();
        
        Console.Write("User ID: ");
        var userIdText = Console.ReadLine();
        if (!int.TryParse(userIdText, out int userId))
        {
            Console.WriteLine("User ID must be a number.");
            return;
        }
        
        //does user exist? 
        bool userExists = userRepository
            .GetManyUsersAsync()
            .Any(u => u.Id == userId);

        if (!userExists)
        {
            Console.WriteLine($"No user found with ID {userId}.");
            return;
        }
        
        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Title cannot be empty.");
            return;
        }
        if (string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Body cannot be empty.");
            return;
        }
       
        bool titleExists = postRepository
            .GetManyPostsAsync()
            .Any(p => string.Equals(p.Title, title, StringComparison.OrdinalIgnoreCase));

        if (titleExists)
        {
            Console.WriteLine("A post with this title already exists.");
            return;
        }

        var now = DateTime.Now;

        var newPost = new Post
        {
            Title = title.Trim(),
            Body = body,
            UserId = userId,
            Created = DateTime.Now,
            Updated = DateTime.Now
        };
     
        Post created = await postRepository.AddPostAsync(newPost);
        
        Console.WriteLine($"Post {created.Id} created");
        
    }
}