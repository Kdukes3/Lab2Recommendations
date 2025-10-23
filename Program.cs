namespace Lab2Recommendations;

class Program
{
    public static void Main(string[] args)
    {
        IBookRepository books = new BookRepository();
        IAccountRepository accounts = new AccountRepository();
        IRatingRepository ratingRepo = new MemoryRatingStore(); 
        IRatingService ratings = new RatingService(ratingRepo);
        IAuthenticator auth = new Authenticator(accounts);
        IRecommendationEngine recs = new Recommender(ratingRepo, books, accounts);
        IDataReader reader = new TextReader();
        IDataSeeder seeder = new DataSeeder(reader, books, accounts, ratingRepo);

        // Hand everything to the UI and run
        var ui = new ConsoleUI(auth, books, accounts, ratingRepo, ratings, recs, reader, seeder);
        ui.Run(); 
    }
}