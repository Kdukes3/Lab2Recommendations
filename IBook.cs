namespace Lab2Recommendations;

public interface IBook
{
    public void Book();
    public Book GetByIbsn(int  isbn);
    public List<Book> GetByIsbn(int isbn);
}