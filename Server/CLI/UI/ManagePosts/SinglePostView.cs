using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView(
    IPostRepository postRepository,
    ICommentRepository commentRepository,
    IUserRepository userRepository)
{
    private readonly IPostRepository _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
    private readonly ICommentRepository _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
    private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

    public async Task RunAsync()
    {
        Console.Clear();
        Console.WriteLine("---- View Post ----\n");

        // show posts
        var posts = _postRepository.GetManyAsync().OrderBy(p => p.Id).ToList();
        if (posts.Count == 0)
        {
            Console.WriteLine("No posts available.");
            return;
        }

        Console.WriteLine("Posts:");
        foreach (var p in posts)
            Console.WriteLine($"{p.Id}: {p.Title} {p.Body}");
        Console.WriteLine();

        // choose postId
        int postId = PromptPositiveInt("Enter post id: ");

        // get post + details
        var post = await TryGetPostAsync(postId);
        if (post == null)
        {
            Console.WriteLine("Post not found.");
            return;
        }

        Console.Clear();
        Console.WriteLine($"Post #{post.Id}: {post.Title}");
        Console.WriteLine(new string('-', 75));
        Console.WriteLine(post.Body);
        Console.WriteLine(new string('-', 75));
        Console.WriteLine($"Created: {post.Created} | Updated: {post.Updated} | Author: User #{post.UserId}\n");

        // get comments
        var users = _userRepository.GetManyAsync().ToList();
        var comments = _commentRepository.GetManyAsync()
            .Where(c => c.PostId == postId)
            .OrderBy(c => c.Id)
            .ToList();

        if (comments.Count == 0)
        {
            Console.WriteLine("No comments yet.");
            return;
        }

        Console.WriteLine("Comments:");
        foreach (var c in comments)
        {
            var author = users.FirstOrDefault(u => u.Id == c.UserId)?.Username ?? $"User #{c.UserId}";
            Console.WriteLine($"\n[{c.Id}] {c.Title} — by {author}");
            Console.WriteLine(c.Body);
            Console.WriteLine($"(Created: {c.Created}, Updated: {c.Updated})");
        }
    }

    private async Task<Entities.Post?> TryGetPostAsync(int id)
    {
        try { return await _postRepository.GetSingleAsync(id); }
        catch { return null; }
    }

    private static int PromptPositiveInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? s = Console.ReadLine();
            if (int.TryParse(s, out int v) && v > 0) return v;
            Console.WriteLine("Please enter a valid positive number.");
        }
    }
}
