using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageComments;

public class CreateCommentView
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICommentRepository _commentRepository;

    public CreateCommentView(IPostRepository postRepository,  ICommentRepository commentRepository, IUserRepository userRepository)
    {
        this._postRepository = postRepository;
        this._commentRepository = commentRepository;
        this._userRepository = userRepository;
    }

    public async Task RunAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Add Comment to Post ===\n");

        //show posts
        var posts = _postRepository.GetManyAsync().OrderBy(p => p.Id).ToList();
        if (posts.Count == 0)
        {
            Console.WriteLine("No posts available yet.");
            return;
        }

        Console.WriteLine("Posts:");
        foreach (var p in posts)
            Console.WriteLine($"{p.Id}: {p.UserId}, {p.Title}, {p.Body}");
        Console.WriteLine();

        // choose postId
        int postId = PromptPositiveInt("Enter post id: ");
        var post = await TryGetPostAsync(postId);
        if (post == null)
        {
            Console.WriteLine("Post not found.");
            return;
        }

        // ask userId
        int userId = PromptPositiveInt("Your user id: ");
        var user = await TryGetUserAsync(userId);
        if (user == null)
        {
            Console.WriteLine("User not found.");
            return;
        }

        // title and body
        string title = PromptNonEmpty("Comment title: ");
        string body  = PromptNonEmpty("Comment body: ");

        var comment = new Comment
        {
            Title   = title.Trim(),
            Body    = body.Trim(),
            UserId  = userId,
            PostId  = postId,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow
        };

        var created = await _commentRepository.AddAsync(comment);
        Console.WriteLine($"\nComment #{created.Id} added to Post #{post.Id} by User #{user.Id}.");
    }

    private static int PromptPositiveInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? s = Console.ReadLine();
            if (int.TryParse(s, out int v) && v > 0)
                return v;
            Console.WriteLine("Please enter a valid positive number.");
        }
    }

    private static string PromptNonEmpty(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
                return input.Trim();
            Console.WriteLine("Please enter a value.");
        }
    }

    private async Task<Post?> TryGetPostAsync(int id)
    {
        try { return await _postRepository.GetSingleAsync(id); }
        catch { return null; }
    }

    private async Task<User?> TryGetUserAsync(int id)
    {
        try { return await _userRepository.GetSingleAsync(id); }
        catch { return null; }
    }
}
