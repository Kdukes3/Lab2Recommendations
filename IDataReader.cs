namespace Lab2Recommendations;

public interface IDataReader
{
    // Loads books and ratings from the provided files into the repositories.
    void Seed(IBookRepository books, IAccountRepository accounts, IRatingService ratings,
        string booksPath, string ratingsPath);
}