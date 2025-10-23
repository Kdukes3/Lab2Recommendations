namespace Lab2Recommendations;

public interface IBookRepository
{
    int Count { get; }
    
    Book Add(Book b);
    Book GetByIsbn(int isbn);  // isbn == 1-based index displayed in UI
    List<Book> All();
    
}