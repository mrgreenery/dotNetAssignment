using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    public CreatePostView(IPostRepository postRepository, IUserRepository userRepository)
    {
        this._postRepository = postRepository;
        this._userRepository = userRepository;
    }
    public async Task RunAsync()
    {
        Console.Clear();
        Console.WriteLine("----Create new post----");
        
        Console.Write("What is the title of you post: ");
        string title = Console.ReadLine();
        
        Console.Write("Write your post: ");
        string body = Console.ReadLine();
        
        Console.Write("User ID: ");
        var userIdText = Console.ReadLine();
        if (!int.TryParse(userIdText, out int userId))
        {
            Console.WriteLine("User ID must be a number.");
            return;
        }

        //does user exist? 
        bool userExists = _userRepository
            .GetManyAsync()
            .Any(u => u.Id == userId);

        if (!userExists)
        {
            Console.WriteLine($"No user found with ID {userId}.");
            return;
        }
        
        var now = DateTime.Now;

        var newPost = new Post(title, body, now) { UserId = userId };
     
        Post created = await _postRepository.AddAsync(newPost);
        
        Console.WriteLine($"Post {created.Id} created");
        
    }
}