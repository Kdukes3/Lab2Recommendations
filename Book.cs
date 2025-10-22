namespace Lab2Recommendations;

public class Book: IBook
{
    private String _Isbn;
    private String _Title;
    private String _Author;
    private String _Year;

    public Book(string isbn, string title, string author, string year)
    {
        _Isbn = isbn;
        _Title = title;
        _Author = author;
        _Year = year;
    }

    public Book GetByIsbn(int isbn)
    {
        //Needs IBookRepository to 
    }
}