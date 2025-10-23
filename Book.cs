namespace Lab2Recommendations;

public class Book
{
    public int _Isbn;
    private String _Title;
    private String _Author;
    private String _Year;

    public Book(int isbn, string title, string author, string year)
    {
        _Isbn = isbn;
        _Title = title;
        _Author = author;
        _Year = year;
    }

    public string Title => _Title;
    public string Author => _Author;
    public string Year => _Year;
}