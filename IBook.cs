namespace Lab2Recommendations;

public interface IBook
{
    public void Book(string isbn, string title, string author, string year);
    public Book GetByIsbn(int isbn);
    //public List<Book> GetByIsbn(int isbn);
}