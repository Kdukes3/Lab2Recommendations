namespace Lab2Recommendations;

public class AddBookAction : IMenuAction
{
    private readonly IBookRepository _books;
    public AddBookAction(IBookRepository books) => _books = books;

    public string Key => "2";
    public string Label => "Add a new book";

    public void Execute()
    {
        Console.Write("Title: ");  var title  = Console.ReadLine() ?? "";
        Console.Write("Author: "); var author = Console.ReadLine() ?? "";
        Console.Write("Year: ");   var year   = Console.ReadLine() ?? "";

        var nextIsbn = _books.Count + 1;
        var b = _books.Add(new Book(nextIsbn, title.Trim(), author.Trim(), year.Trim()));
        Console.WriteLine($"Book #{b._Isbn} - '{b.Title}' by {b.Author} ({b.Year}) added.\n");
    }
}