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

        // Hand everything to the UI and run
        var ui = new ConsoleUI(auth, books, accounts, ratings, recs, reader);
        ui.Run(); 
    }
}