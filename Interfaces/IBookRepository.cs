namespace Lab2Recommendations;

public interface IBookRepository
{
    int Count { get; }
    
    Book Add(Book b);
    Book GetByIsbn(int isbn); 
    List<Book> All();
    
}