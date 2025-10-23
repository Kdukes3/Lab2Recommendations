namespace Lab2Recommendations;

public class ConsoleUI
{
    private readonly IAuthenticator _auth;
    private readonly IBookRepository _books;
    private readonly IAccountRepository _accounts;
    private readonly IRatingService _ratings;
    private readonly IRecommendationEngine _recs;
    private readonly IDataReader _reader;

    public bool IsRunning { get; private set; }

    public ConsoleUI(
        IAuthenticator auth,
        IBookRepository books,
        IAccountRepository accounts,
        IRatingService ratings,
        IRecommendationEngine recs,
        IDataReader reader)
    {
        _auth = auth;
        _books = books;
        _accounts = accounts;
        _ratings = ratings;
        _recs = recs;
        _reader = reader;
    }

    public void Start() => IsRunning = true;
    public void Stop()  => IsRunning = false;

    public bool Run()
    {
        Start();
        Console.WriteLine("Welcome to our Book Recommendation System!\n");

        // Build main menu
        var mainActions = new List<IMenuAction>
        {
            new AddMemberAction(_accounts),
            new AddBookAction(_books),
            new LoginAction(_auth, onLoginSuccess: () => { /* no-op here */ }),
            new QuitAction(Stop)
        };

        // Build logged-in menu
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
}
