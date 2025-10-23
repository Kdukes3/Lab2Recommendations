using Lab2Recommendations.Models;

namespace Lab2Recommendations.Interfaces;

public interface IBookRepository
{
    int Count { get; }
    
    Book Add(Book b);
    Book GetByIsbn(int isbn); 
    List<Book> All();
    
}