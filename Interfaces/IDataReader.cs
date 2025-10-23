namespace Lab2Recommendations.Interfaces;

public interface IDataReader
{
    // Loads books and ratings from the provided files into the repositories.
    void Seed(IBookRepository books, IAccountRepository accounts, IRatingRepository ratings,
        string booksPath, string ratingsPath);
}