using Lab2Recommendations.Interfaces;

namespace Lab2Recommendations.Utility;

public class DataSeeder : IDataSeeder
{
    private readonly IDataReader _reader;
    private readonly IBookRepository _books;
    private readonly IAccountRepository _accounts;
    private readonly IRatingRepository _ratings;

    public DataSeeder(IDataReader reader, IBookRepository books, IAccountRepository accounts, IRatingRepository ratings)
    {
        _reader = reader;
        _books = books;
        _accounts = accounts;
        _ratings = ratings;
    }

    public SeedResult Seed(string booksPath, string ratingsPath)
    {
        if (string.IsNullOrWhiteSpace(booksPath) || string.IsNullOrWhiteSpace(ratingsPath))
        {
            return new SeedResult(false, "No files provided. Starting with no data.");
        }

        try
        {
            _reader.Seed(_books, _accounts, _ratings, booksPath, ratingsPath);
            return new SeedResult(true, "Data loaded successfully.");
        }

        catch (FileNotFoundException e)
        {
            return new SeedResult(false, "File not found. Starting with no data.");
        }

        catch (DirectoryNotFoundException e)
        {
            return new SeedResult(false, "Directory not found. Starting with no data.");
        }

        catch (Exception e)
        {
            return new SeedResult(false, $"Failed to load data: {e.Message}\nStarting with no data.");
        }
    }
}