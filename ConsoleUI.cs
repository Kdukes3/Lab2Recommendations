namespace Lab2Recommendations;

public class ConsoleUI
{
    private readonly IAuthenticator _auth;
    private readonly IBookRepository _books;
    private readonly IAccountRepository _accounts;
    private readonly IRatingService _ratings;
    private readonly IRecommendationEngine _recs;
    private readonly IDataReader _reader;

    public ConsoleUI(IAuthenticator auth, 
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

    // -------------------------------------------------------------
    // MAIN PROGRAM LOOP
    // -------------------------------------------------------------
    public void Run()
    {
        Console.WriteLine("Welcome to the Book Recommendation Program.\n");

        Console.Write("Enter books file: ");
        string booksFile = Console.ReadLine();

        Console.Write("Enter rating file: ");
        string ratingsFile = Console.ReadLine();

        _reader.Seed(_books, _accounts, _ratings, booksFile, ratingsFile);

        Console.WriteLine($"\n# of books: {_books.Count}");
        Console.WriteLine($"# of memberList: {_accounts.Count}\n");

        while (true)
        {
            if (!_auth.IsLoggedIn)
            {
                ShowMainMenu();
            }
            else
            {
                ShowMemberMenu();
            }
        }
    }

    // -------------------------------------------------------------
    // MENU SCREENS
    // -------------------------------------------------------------
    private void ShowMainMenu()
    {
        Console.WriteLine("************** MENU **************");
        Console.WriteLine("* 1. Add a new member            *");
        Console.WriteLine("* 2. Add a new book              *");
        Console.WriteLine("* 3. Login                       *");
        Console.WriteLine("* 4. Quit                        *");
        Console.WriteLine("**********************************");
        Console.Write("\nEnter a menu option: ");

        string input = Console.ReadLine();

        switch (input)
        {
            case "1": AddMember(); break;
            case "2": AddBook(); break;
            case "3": Login(); break;
            case "4":
                Console.WriteLine("\nThank you for using the Book Recommendation Program!");
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid option.\n");
                break;
        }
    }

    private void ShowMemberMenu()
    {
        Console.WriteLine("************** MENU **************");
        Console.WriteLine("* 1. Add a new member            *");
        Console.WriteLine("* 2. Add a new book              *");
        Console.WriteLine("* 3. Rate book                   *");
        Console.WriteLine("* 4. View ratings                *");
        Console.WriteLine("* 5. See recommendations         *");
        Console.WriteLine("* 6. Logout                      *");
        Console.WriteLine("**********************************");
        Console.Write("\nEnter a menu option: ");

        string input = Console.ReadLine();

        switch (input)
        {
            case "1": AddMember(); break;
            case "2": AddBook(); break;
            case "3": RateBook(); break;
            case "4": ViewRatings(); break;
            case "5": ShowRecommendations(); break;
            case "6": _auth.Logout(); Console.WriteLine(); break;
            default: Console.WriteLine("Invalid option.\n"); break;
        }
    }

    // -------------------------------------------------------------
    // MENU ACTIONS
    // -------------------------------------------------------------
    private void AddMember()
    {
        Console.Write("Enter the name of the new member: ");
        string name = Console.ReadLine();

        var newMember = _accounts.Add(name);
        Console.WriteLine($"{newMember.Name} (account #: {newMember.Id}) was added.\n");
    }

    private void AddBook()
    {
        Console.Write("Enter the author of the new book: ");
        string author = Console.ReadLine();
        Console.Write("Enter the title of the new book: ");
        string title = Console.ReadLine();
        Console.Write("Enter the year (or range of years) of the new book: ");
        string year = Console.ReadLine();

        var newBook = _books.Add(author, title, year);
        Console.WriteLine($"{newBook.Id}, {author}, {title}, {year} was added.\n");
    }

    private void Login()
    {
        Console.Write("\nEnter member account: ");
        string input = Console.ReadLine();

        if (int.TryParse(input, out int id))
        {
            var member = _accounts.GetById(id);
            if (member != null)
            {
                _auth.Login(member);
                Console.WriteLine($"{member.Name}, you are logged in!\n");
            }
            else Console.WriteLine("Account not found.\n");
        }
    }

    private void RateBook()
    {
        var member = _auth.CurrentMember;
        if (member == null) return;

        Console.Write("\nEnter the ISBN for the book you'd like to rate: ");
        if (int.TryParse(Console.ReadLine(), out int isbn))
        {
            var book = _books.GetByIsbn(isbn);
            if (book == null)
            {
                Console.WriteLine("Book not found.\n");
                return;
            }

            int currentRating = _ratings.GetRating(member.Id, isbn);
            if (currentRating != 0)
            {
                Console.WriteLine($"Your current rating for {isbn}, {book.Author}, {book.Title}, {book.Year} => rating: {currentRating}");
                Console.Write("Would you like to re-rate this book (y/n)? ");
                if (Console.ReadLine()?.ToLower() != "y") return;
            }

            Console.Write("Enter your rating: ");
            if (int.TryParse(Console.ReadLine(), out int value))
            {
                _ratings.SetRating(member.Id, isbn, value);
                Console.WriteLine($"Your new rating for {isbn}, {book.Author}, {book.Title}, {book.Year} => rating: {value}\n");
            }
        }
    }

    private void ViewRatings()
    {
        var member = _auth.CurrentMember;
        if (member == null) return;

        Console.WriteLine($"\n{member.Name}'s ratings...");
        for (int isbn = 1; isbn <= _books.Count; isbn++)
        {
            var book = _books.GetByIsbn(isbn);
            int rating = _ratings.GetRating(member.Id, isbn);
            Console.WriteLine($"{isbn}, {book.Author}, {book.Title}, {book.Year} => rating: {rating}");
        }
        Console.WriteLine();
    }

    private void ShowRecommendations()
    {
        var member = _auth.CurrentMember;
        if (member == null) return;

        var result = _recs.RecommendFromNearest(member.Id);
        if (result.neighbor == null)
        {
            Console.WriteLine("\nNo suitable neighbor found for recommendations.\n");
            return;
        }

        Console.WriteLine($"\nYou have similar taste in books as {result.neighbor.Name}!");
        Console.WriteLine("\nHere are the books they really liked:");
        foreach (var b in result.reallyLiked)
            Console.WriteLine($"{b.Id}, {b.Author}, {b.Title}, {b.Year}");

        Console.WriteLine("\nAnd here are the books they liked:");
        foreach (var b in result.liked)
            Console.WriteLine($"{b.Id}, {b.Author}, {b.Title}, {b.Year}");
        Console.WriteLine();
    }
}
