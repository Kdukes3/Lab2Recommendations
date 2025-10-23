namespace Lab2Recommendations;

public class ConsoleUI
{
    private readonly IAuthenticator _auth;
    private readonly IBookRepository _books;
    private readonly IAccountRepository _accounts;
    private readonly IRatingService _ratings;
    private readonly IRecommendationEngine _recs;
    private readonly IDataReader _reader;

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
        _ratings = ratings;   // <-- this was missing before
        _recs = recs;
        _reader = reader;
    }

    public bool IsRunning { get; private set; } = false;

    public void Start() => IsRunning = true;
    public void Stop()  => IsRunning = false;

    /// <summary>
    /// Runs the entire console UI: welcome, main menu loop, and (after login) the logged-in loop.
    /// This method does not return until the user chooses "Quit".
    /// </summary>
    public bool Run()
    {
        Start();
        ShowWelcome();

        // If you want to load data files at start, uncomment this block and implement _reader.Load(...)
        // var (booksPath, ratingsPath) = PromptFilePaths();
        // var (bookCount, memberCount) = _reader.Load(booksPath, ratingsPath, _books, _accounts, _ratings);
        // ShowCounts(bookCount, memberCount);

        while (IsRunning)
        {
            var choice = ShowMainMenuAndGetChoice();

            switch (choice)
            {
                case MainMenuOption.AddMember:
                {
                    var name = PromptForMemberName();

                    // TODO: actually create a member in your accounts repository.
                    // e.g., var id = _accounts.Create(name);
                    ShowMessage($"Member '{name}' added. (stub)");
                    break;
                }

                case MainMenuOption.AddBook:
                {
                    var (title, author, year) = PromptForBookDetails();

                    // TODO: actually add a book to your repository.
                    // e.g., _books.Add(new Book { Title = title, Author = author, Year = year });
                    ShowMessage($"Book '{title}' by {author} ({year}) added. (stub)");
                    break;
                }

                case MainMenuOption.Login:
                {
                    var memberId = PromptForMemberId();

                    // TODO: replace this with your real auth call pattern.
                    // If your authenticator returns a bool:
                    // var ok = _auth.Login(memberId);
                    // If it returns a result object, adjust accordingly.
                    var loginSucceeded = TryLogin(memberId);

                    if (loginSucceeded)
                    {
                        ShowLoginSuccess($"Login successful for member #{memberId}.");
                        LoggedInLoop(memberId);
                    }
                    else
                    {
                        ShowLoginFailure("Login failed. Please check your member ID.");
                    }
                    break;
                }

                case MainMenuOption.Quit:
                {
                    Stop();
                    ShowMessage("Goodbye!");
                    break;
                }

                default:
                {
                    ShowMessage("Invalid option. Try again.");
                    break;
                }
            }
        }

        return true; // indicates the app ran to completion
    }

    // ---------------------
    // Logged-in sub-loop
    // ---------------------
    private void LoggedInLoop(int memberId)
    {
        var inSession = true;

        while (inSession)
        {
            var choice = ShowLoggedInMenuAndGetChoice();

            switch (choice)
            {
                case LoggedInMenuOption.AddMember:
                {
                    var name = PromptForMemberName();
                    // TODO: create another member (admin-like action?)
                    ShowMessage($"Member '{name}' added. (stub)");
                    break;
                }

                case LoggedInMenuOption.AddBook:
                {
                    var (title, author, year) = PromptForBookDetails();
                    // TODO: add book to repository
                    ShowMessage($"Book '{title}' by {author} ({year}) added. (stub)");
                    break;
                }

                case LoggedInMenuOption.RateBook:
                {
                    // TODO: prompt for book id & rating, then call _ratings.AddOrUpdate(memberId, bookId, score)
                    ShowMessage("Rating flow not implemented yet. (stub)");
                    break;
                }

                case LoggedInMenuOption.ViewRatings:
                {
                    // TODO: fetch and print this member’s ratings via _ratings
                    ShowMessage("View ratings not implemented yet. (stub)");
                    break;
                }

                case LoggedInMenuOption.SeeRecommendations:
                {
                    // TODO: get recs via _recs for this member and display them
                    ShowMessage("Recommendations not implemented yet. (stub)");
                    break;
                }

                case LoggedInMenuOption.Logout:
                {
                    // TODO: if your authenticator needs explicit logout, call it here.
                    // _auth.Logout(memberId);
                    ShowMessage("Logged out.");
                    inSession = false;
                    break;
                }

                default:
                {
                    ShowMessage("Invalid option. Try again.");
                    break;
                }
            }
        }
    }

    // Replace this shim with the real call to your authenticator to avoid compile issues if signatures differ.
    private bool TryLogin(int memberId)
    {
        try
        {
            // If your authenticator exposes bool Login(int id), just return that:
            // return _auth.Login(memberId);

            // Placeholder behavior (always succeeds for demo):
            return true;
        }
        catch
        {
            return false;
        }
    }

    // ------------- Existing I/O helpers (with a couple of small fixes) -------------

    public void ShowWelcome()
    {
        Console.WriteLine("Welcome to our Book Recommendation System!");
        Console.WriteLine("");
    }

    public (string booksPath, string ratingsPath) PromptFilePaths()
    {
        Console.WriteLine("");
        Console.Write("Enter books file: ");
        var books = Console.ReadLine()?.Trim() ?? string.Empty;

        Console.Write("Enter ratings file: ");
        var ratings = Console.ReadLine()?.Trim() ?? string.Empty;

        Console.WriteLine("");
        Console.WriteLine($"Enter books file: {books}");
        Console.WriteLine($"Enter ratings file: {ratings}");
        Console.WriteLine("");
        return (books, ratings);
    }

    public void ShowCounts(int bookCount, int memberCount)
    {
        Console.WriteLine($"# of books: {bookCount}");
        Console.WriteLine($"# of members: {memberCount}");
        Console.WriteLine("");
    }

    public MainMenuOption ShowMainMenuAndGetChoice()
    {
        Console.WriteLine("************* MENU *************");
        Console.WriteLine("* 1. Add a new member         *");
        Console.WriteLine("* 2. Add a new book           *");
        Console.WriteLine("* 3. Login                    *");
        Console.WriteLine("* 4. Quit                     *");
        Console.WriteLine("*******************************");
        Console.Write("");
        Console.Write("Enter a menu option: ");
        var input = Console.ReadLine()?.Trim() ?? string.Empty;

        Console.WriteLine("");
        Console.WriteLine($"Enter a menu option: {input}");
        Console.WriteLine("");

        return input switch
        {
            "1" => MainMenuOption.AddMember,
            "2" => MainMenuOption.AddBook,
            "3" => MainMenuOption.Login,
            "4" => MainMenuOption.Quit,
            _ => MainMenuOption.None
        };
    }

    public LoggedInMenuOption ShowLoggedInMenuAndGetChoice()
    {
        Console.WriteLine("************* MENU *************");
        Console.WriteLine("* 1. Add a new member         *");
        Console.WriteLine("* 2. Add a new book           *");
        Console.WriteLine("* 3. Rate a book              *");
        Console.WriteLine("* 4. View your ratings        *");
        Console.WriteLine("* 5. See recommendations      *");
        Console.WriteLine("* 6. Logout                   *");
        Console.WriteLine("*******************************");
        Console.Write("");
        Console.Write("Enter a menu option: ");
        var input = Console.ReadLine()?.Trim() ?? string.Empty;

        Console.WriteLine("");
        Console.WriteLine($"Enter a menu option: {input}");
        Console.WriteLine("");

        return input switch
        {
            "1" => LoggedInMenuOption.AddMember,
            "2" => LoggedInMenuOption.AddBook,
            "3" => LoggedInMenuOption.RateBook,
            "4" => LoggedInMenuOption.ViewRatings,
            "5" => LoggedInMenuOption.SeeRecommendations,
            "6" => LoggedInMenuOption.Logout,
            _ => LoggedInMenuOption.None
        };
    }

    public int PromptForMemberId()
    {
        while (true)
        {
            Console.Write("Enter member account ID: ");
            var input = Console.ReadLine()?.Trim() ?? string.Empty;
            Console.WriteLine($"Enter member account ID: {input}");

            if (int.TryParse(input, out var id))
                return id;

            ShowMessage("Invalid number. Try again.");
        }
    }

    public string PromptForMemberName()
    {
        Console.Write("Enter member name: ");
        var name = Console.ReadLine()?.Trim() ?? string.Empty;
        Console.WriteLine($"Enter member name: {name}");
        return name;
    }

    public (string title, string author, string year) PromptForBookDetails() // fixed name
    {
        Console.Write("Title: ");
        var title = Console.ReadLine() ?? string.Empty;
        Console.Write("Author: ");
        var author = Console.ReadLine() ?? string.Empty;
        Console.Write("Year: ");
        var year = Console.ReadLine() ?? string.Empty;

        Console.WriteLine($"Title: {title}");
        Console.WriteLine($"Author: {author}");
        Console.WriteLine($"Year: {year}");

        return (title, author, year);
    }

    public void ShowLoginSuccess(string message)
    {
        Console.WriteLine(message);
        Console.WriteLine("");
    }

    public void ShowLoginFailure(string message)
    {
        Console.WriteLine(message);
        Console.WriteLine("");
    }

    public void ShowMessage(string message)
    {
        Console.WriteLine(message);
        Console.WriteLine("");
    }
}
