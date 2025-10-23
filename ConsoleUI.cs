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
        _ratings = ratings;
        _recs = recs;
        _reader = reader;
    }

    public bool IsRunning { get; private set; } = false;

    public void Start() => IsRunning = true;
    public void Stop()  => IsRunning = false;

    public bool Run()
    {
        Start();
        ShowWelcome();

        while (IsRunning)
        {
            var choice = ShowMainMenuAndGetChoice();

            switch (choice)
            {
                case MainMenuOption.AddMember:
                {
                    var name = PromptForMemberName(); 
                    var member = _accounts.GetOrAdd(name);
                    ShowMessage($"Member '{member.Name}' added with ID {member._Account}");
                    break;
                }

                case MainMenuOption.AddBook:
                {
                    var (title, author, year) = PromptForBookDetails();
                    var nextIsbn = _books.Count + 1;
                    var added = _books.Add(new Book(nextIsbn, title, author, year));
                    ShowMessage($"Book #{added._Isbn} - '{added.Title}' by {added.Author} ({added.Year}) added.");
                    break;
                }

                case MainMenuOption.Login:
                {
                    Console.Write("Enter member ID or name: ");
                    var raw = Console.ReadLine()?.Trim() ?? "";
                    bool ok;

                    if (int.TryParse(raw, out var id))
                        ok = _auth.LoginById(id);
                    else
                        ok = _auth.LoginByName(raw);

                    if (ok)
                    {
                        ShowLoginSuccess(_auth.Message);
                        LoggedInLoop(_auth.CurrentId);
                    }
                    else
                    {
                        ShowLoginFailure(_auth.Message);
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

        return true; // app ran to completion
    }
    
    // Logged-in sub-loop
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
                    var m = _accounts.GetOrAdd(name);
                    ShowMessage($"Member '{m.Name}' added. {m._Account}");
                    break;
                }

                case LoggedInMenuOption.AddBook:
                {
                    var (title, author, year) = PromptForBookDetails();
                    var nextIsbn = _books.Count + 1;
                    var b = _books.Add(new Book(nextIsbn, title, author, year));
                    ShowMessage($"Book #{b._Isbn} - '{b.Title}' by {b.Author} ({b.Year}) added.");
                    break;
                }

                case LoggedInMenuOption.RateBook:
                {
                    var bookId = PromptForPositiveInt("Enter book ISBN to rate: ");
                    var rating = PromptForIntInRange("Enter rating (-5 to 5): ", -5, 5);

                    _ratings.SetRating(memberId, bookId, rating);

                    var b = _books.GetByIsbn(bookId);
                    var label = b != null ? $"'{b.Title}'" : $"Book #{bookId}";
                    ShowMessage($"Rated {label} as {rating}."); 
                    break;
                }

                case LoggedInMenuOption.ViewRatings:
                {
                    var ratings = _ratings.GetRatingsForMember(memberId);
                    if (ratings.Count == 0)
                    {
                        ShowMessage("You have no ratings yet.");
                        break;
                    }

                    Console.WriteLine("Your ratings:");
                    foreach (var rating in ratings)
                    {
                        var b = _books.GetByIsbn(rating.isbn);
                        var label = b != null ? $"'{b.Title}'" : $"Book #{rating.isbn}";
                        Console.WriteLine($"- {label}: {rating.value}");
                    }
                    Console.WriteLine("");
                    break;
                }

                case LoggedInMenuOption.SeeRecommendations:
                {
                    var (neighbor, reallyLiked, liked) = _recs.RecommendFromNearest(memberId);
                    if (neighbor == null)
                    {
                        ShowMessage("No neighbor found yet. Add more member/ratings.");
                        break;
                    }
                    
                    Console.WriteLine($"Nearest neighbor: {neighbor.Name} (ID {neighbor._Account})");
                    Console.WriteLine("They really liked (5):");
                    if (reallyLiked.Count == 0) Console.WriteLine("- none -");
                    foreach (var b in reallyLiked) Console.WriteLine($"- {b._Isbn}: {b.Title}");
                    
                    Console.WriteLine("They liked (1 or 3):");
                    if (liked.Count == 0) Console.WriteLine("- none -");
                    foreach (var b in liked) Console.WriteLine($"- {b._Isbn}: {b.Title}");
                    Console.WriteLine("");
                    break;
                }

                case LoggedInMenuOption.Logout:
                {
                    _auth.Logout();
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

    private int PromptForPositiveInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim() ?? "";   
            if (int.TryParse(input, out var value) && value > 0)
                return value;
            ShowMessage("Invalid number. Try again.");
        }
    }

    private int PromptForIntInRange(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim() ?? "";
            if (int.TryParse(input, out var value) && value >= min && value <= max)
                return value;
            ShowMessage($"Please enter a number between {min} and {max}");
        }
    }

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

    public (string title, string author, string year) PromptForBookDetails()
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