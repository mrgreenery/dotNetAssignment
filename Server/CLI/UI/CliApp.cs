using CLI.UI.ManageComments;
using CLI.UI.ManagePosts;
using CLI.UI.ManageUsers;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp(
    IUserRepository userRepository,
    ICommentRepository commentRepository,
    IPostRepository postRepository)
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IPostRepository _postRepository = postRepository;
    //
    // private CreatePostView? createPostView;
    // private SinglePostView? singlePostView;
    // private CreateUserView? createUserView;
    // private ListUsersView? listUsersView;
    // private CreateCommentView? createCommentView;

    public async Task StartAsync()
    { 
        while (true) 
        { 
            PrintMenu();
            int choice = ReadMenuChoice(1, 6);
            
            try 
            { 
                switch (choice)
                {
                case 1:
                    //instantiate the views here
                    CreateUserView createUserView = new CreateUserView(_userRepository);
                    await createUserView.RunAsync();
                    break;

                case 2:
                    CreatePostView createPostView = new CreatePostView(_postRepository, userRepository);
                    await createPostView.RunAsync();
                    break;

                case 3:
                    CreateCommentView createCommentView = new CreateCommentView(_postRepository, _commentRepository, userRepository);
                    await createCommentView.RunAsync();
                    break;

                case 4:
                    SinglePostView singlePostView = new SinglePostView(_postRepository, _commentRepository, _userRepository);
                    await singlePostView.RunAsync();
                    break;

                case 5:
                    Console.WriteLine("ByeBye now!");
                    return; 
                }
            }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("\nPress ENTER to return to the menu...");
        Console.ReadLine(); 
    }
    }

private static void PrintMenu()
{
    Console.Clear();
    Console.WriteLine("------------ MENU ------------");
    Console.WriteLine("1. Create a new user");
    Console.WriteLine("2. Create a new post");
    Console.WriteLine("3. Add a comment to a post");
    Console.WriteLine("4. Show all posts");
    Console.WriteLine("5. Exit");
    Console.Write("\nChoose a number (1–6) and press ENTER: ");
}

private static int ReadMenuChoice(int min, int max)
{
    while (true)
    {
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.Write($"Please enter a number {min}–{max}: ");
            continue;
        }

        if (!int.TryParse(input.Trim(), out int number))
        {
            Console.Write($"Not a number. Choose {min}–{max}: ");
            continue;
        }

        if (number < min || number > max)
        {
            Console.Write($"Out of range. Choose {min}–{max}: ");
            continue;
        }

        return number;
    }
}

}