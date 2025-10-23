namespace Lab2Recommendations;

public class RateBookAction : IMenuAction
{
    private readonly IRatingService _ratings;
    private readonly IBookRepository _books;
    private readonly Func<int> _memberId;

    public RateBookAction(IRatingService ratings, IBookRepository books, Func<int> memberId)
    {
        _ratings = ratings; _books = books; _memberId = memberId;
    }

    public string Key => "3";
    public string Label => "Rate a book";

    public void Execute()
    {
        Console.Write("Enter book ISBN to rate: ");
        var bookRaw = (Console.ReadLine() ?? "").Trim();
        if (!int.TryParse(bookRaw, out var isbn) || isbn <= 0) { Console.WriteLine("Invalid ISBN.\n"); return; }

        Console.Write("Enter rating (-5 to 5): ");
        var rateRaw = (Console.ReadLine() ?? "").Trim();
        if (!int.TryParse(rateRaw, out var rating) || rating < -5 || rating > 5) { Console.WriteLine("Invalid rating.\n"); return; }

        _ratings.SetRating(_memberId(), isbn, rating);
        var b = _books.GetByIsbn(isbn);
        var label = b != null ? $"'{b.Title}'" : $"Book #{isbn}";
        Console.WriteLine($"Rated {label} as {rating}.\n");
    }
}