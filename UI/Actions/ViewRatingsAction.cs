namespace Lab2Recommendations;

public class ViewRatingsAction : IMenuAction
{
    private readonly IRatingService _ratings;
    private readonly IBookRepository _books;
    private readonly Func<int> _memberId;

    public ViewRatingsAction(IRatingService ratings, IBookRepository books, Func<int> memberId)
    {
        _ratings = ratings; _books = books; _memberId = memberId;
    }

    public string Key => "4";
    public string Label => "View your ratings";

    public void Execute()
    {
        var list = _ratings.GetRatingsForMember(_memberId());
        if (list.Count == 0) { Console.WriteLine("You have no ratings yet.\n"); return; }

        Console.WriteLine("Your ratings:");
        foreach (var r in list)
        {
            var b = _books.GetByIsbn(r.isbn);
            var label = b != null ? $"'{b.Title}'" : $"Book #{r.isbn}";
            Console.WriteLine($"- {label}: {r.value}");
        }
        Console.WriteLine("");
    }
}