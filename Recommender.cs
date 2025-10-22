namespace Lab2Recommendations;

public class Recommender : IRecommendationEngine
{
    private readonly IRatingRepository _ratings;
    private readonly IBookRepository _books;
    private readonly IAccountRepository _accounts;

    public Recommender(IRatingRepository ratings, IBookRepository books, IAccountRepository accounts)
    {
        _ratings = ratings;
        _books = books;
        _accounts = accounts;
    }
    
    public (Member neighbor, List<Book> reallyLiked, List<Book> liked) RecommendFromNearest(int memberId)
    {
        // 1) Pick the single best neighbor
        Member bestNeighbor = FindBestNeighbor(memberId);

        // If no useful neighbor, return empty lists
        if (bestNeighbor == null)
            return (null, new List<Book>(), new List<Book>());

        // 2) Collect recommendations from that neighbor
        var really = new List<Book>();
        var liked  = new List<Book>();
        
        for (int isbn = 1; isbn <= _books.Count; isbn++)
        {
            // We do not recommend books the current user has already rated
            int myRating = _ratings.Get(memberId, isbn);
            if (myRating != 0) continue;

            int neighborRating = _ratings.Get(bestNeighbor._Account, isbn);

            // Really liked (rating 5)
            if (neighborRating == 5)
            {
                AddIfBookExists(really, isbn);
            }
            // Liked (ratings 3 or 1)
            else if (neighborRating == 3 || neighborRating == 1)
            {
                AddIfBookExists(liked, isbn);
            }
        }

        return (bestNeighbor, really, liked);
    }

    private Member FindBestNeighbor(int memberId)
    {
        Member best = null;
        double bestWeight = 0.0;

        var everyone = _accounts.All();
        for (int i = 0; i < everyone.Count; i++)
        {
            var other = everyone[i];
            if (other._Account == memberId) continue;

            double weight = similarity(memberId, other._Account);
            
            if (weight > bestWeight)
            {
                bestWeight = weight;
                best = other;
            }
        }

        // If similarity is 0 or we never found anyone, return null
        if (bestWeight <= 0.0) return null;
        return best;
    }
    
    private void AddIfBookExists(List<Book> target, int isbn)
    {
        Book b = _books.GetByIsbn(isbn);
        if (b != null) target.Add(b);
    }
    
    private double similarity(int aId, int bId)
    {
        double dot = 0.0;
        double normA = 0.0;
        double normB = 0.0;
        bool anyCommon = false;

        for (int isbn = 1; isbn <= _books.Count; isbn++)
        {
            int a = _ratings.Get(aId, isbn);
            int b = _ratings.Get(bId, isbn);

            // Only consider books both actually rated (non-zero)
            if (a == 0 || b == 0) continue;

            anyCommon = true;
            dot   += a * b;
            normA += a * a;
            normB += b * b;
        }

        if (!anyCommon || normA == 0.0 || normB == 0.0) return 0.0;

        double denom = Math.Sqrt(normA) * Math.Sqrt(normB);
        if (denom == 0.0) return 0.0;

        return dot / denom;
    } 
}
