using Lab2Recommendations.Interfaces;
using Lab2Recommendations.Models;

namespace Lab2Recommendations.Services;

public class BookRepository : IBookRepository
{
    private readonly List<Book> _books = new List<Book>();
    public int Count => _books.Count;

    public Book Add(Book b)
    {
        _books.Add(b);
        return b;
    }

    public Book GetByIsbn(int isbn)
    {
        for (int i = 0; i < _books.Count; i++)
            if (_books[i]._Isbn == isbn)
            {


                return _books[i];
            }
        return null;
    }

    public List<Book> All() => _books;
}