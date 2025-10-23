namespace Lab2Recommendations;

public interface IDataSeeder
{
    SeedResult Seed(string booksPath, string ratingsPath);
}

public sealed class SeedResult
{
    public bool Success { get; }
    public string Message { get; }

    public SeedResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}