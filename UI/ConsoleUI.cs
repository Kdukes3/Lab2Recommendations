using Lab2Recommendations.Interfaces;
using Lab2Recommendations.Models;
using Lab2Recommendations.UI.Actions;


namespace Lab2Recommendations.UI;

public class ConsoleUI
{
    private readonly IAuthenticator _auth;
    private readonly IBookRepository _books;
    private readonly IAccountRepository _accounts;
    private readonly IRatingRepository _ratingRepo;
    private readonly IRatingService _ratings;
    private readonly IRecommendationEngine _recs;
    private readonly IDataReader _reader;
    private readonly IDataSeeder _seeder;

    public bool IsRunning { get; private set; }

    public ConsoleUI(
        IAuthenticator auth,
        IBookRepository books,
        IAccountRepository accounts,
        IRatingRepository ratingRepo,
        IRatingService ratings,
        IRecommendationEngine recs,
        IDataReader reader,
        IDataSeeder seeder)
    {
        _auth = auth;
        _books = books;
        _accounts = accounts;
        _ratingRepo = ratingRepo;
        _ratings = ratings;
        _recs = recs;
        _reader = reader;
        _seeder = seeder;
    }

    public void Start() => IsRunning = true;
    public void Stop()  => IsRunning = false;

    public bool Run()
    {
        Start();
        Console.WriteLine("Welcome to our Book Recommendation System!\n");
        
        // File path prompts
        var (booksPath, ratingsPath) = PromptFilePaths();
        var seedResult = _seeder.Seed(booksPath, ratingsPath);
        Console.WriteLine(seedResult.Message + "\n");
        ShowCounts(_books.Count, _accounts.Count);

        // main menu
        var mainActions = new List<IMenuAction>
        {
            new AddMemberAction(_accounts),
            new AddBookAction(_books),
            new LoginAction(_auth, onLoginSuccess: () => { }),
            new QuitAction(Stop)
        };

        // logged-in menu
        var loggedActions = new List<IMenuAction>
        {
            new AddMemberAction(_accounts),
            new AddBookAction(_books),
            new RateBookAction(_ratings, _books, () => _auth.CurrentId),
            new ViewRatingsAction(_ratings, _books, () => _auth.CurrentId),
            new SeeRecommendationsAction(_recs, () => _auth.CurrentId),
            new LogoutAction(_auth)
        };

        while (IsRunning)
        {
            if (!_auth.IsLoggedIn)
                RunMenu("************* MENU *************", mainActions, () => IsRunning && !_auth.IsLoggedIn);
            else
                RunMenu("************* MENU *************", loggedActions, () => IsRunning && _auth.IsLoggedIn);
        }

        return true;
    }

    private void RunMenu(string title, List<IMenuAction> actions, Func<bool> shouldContinue)
    {
        while (shouldContinue())
        {
            Console.WriteLine(title);
            foreach (var a in actions)
                Console.WriteLine($"* {a.Key}. {a.Label}");
            Console.WriteLine("*******************************");
            Console.Write("Enter a menu option: ");

            var input = (Console.ReadLine() ?? "").Trim();
            var action = actions.FirstOrDefault(a => a.Key == input);

            if (action != null) action.Execute();
            else
            {
                Console.WriteLine("Invalid option. Try again.\n");
            }
        }
    }

    public (string booksPath, string ratingsPath) PromptFilePaths()
    {
        Console.Write("Enter books file path: ");
        var books = Console.ReadLine()?.Trim() ?? string.Empty;

        Console.Write("Enter rating file path: ");
        var ratings = Console.ReadLine()?.Trim() ?? string.Empty;

        Console.WriteLine("");
        return (books, ratings);
    }

    public void ShowCounts(int bookCount, int memberCount)
    {
        Console.WriteLine($"# of books: {bookCount}");
        Console.WriteLine($"# of members: {memberCount}");
        Console.WriteLine("");
    }
}
